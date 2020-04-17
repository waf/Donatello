using Donatello.Ast;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;

namespace Donatello.TypeInference
{
	internal static class TypeUnifier
    {
        public static IDictionary<string, IType> UnifyAll(IImmutableList<IConstraint> constraints)
        {
            //var unions = constraints.OfType<UnionConstraint>().ToList();
            //var concretes = constraints.OfType<Constraint>().ToList();

            //// TODO: this is terrible! 
            //var product = CartesianProduct(unions.Select(u => u.Constraints));
            //var possibilities = from possibility in product
            //                    select concretes.Concat(possibility).ToImmutableList();

            //var results = possibilities.Select(Unify).ToList();

            //return results.Single(r => r != null);
            var result = Unify(constraints);
            return result.Single();
        }

        //static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        //{
        //    IEnumerable<IEnumerable<T>> emptyProduct =
        //      new[] { Enumerable.Empty<T>() };
        //    return sequences.Aggregate(
        //      emptyProduct,
        //      (accumulator, sequence) =>
        //        from accseq in accumulator
        //        from item in sequence
        //        select accseq.Concat(new[] { item }));
        //}

        public static IEnumerable<IDictionary<string, IType>> Unify(IImmutableList<IConstraint> constraints)
        {
            Console.WriteLine(Indent() + "Unify: " + string.Join("; ", constraints));

            if (!constraints.Any())
                return new[] { new Dictionary<string, IType>() };

            var head = constraints.First();
            var tail = constraints.Skip(1).ToImmutableList();
            IEnumerable<IDictionary<string, IType>> t2Solutions = Unify(tail);
            if (t2Solutions == null) // no solution
                return null;

            IEnumerable<IDictionary<string, IType>> t1Solutions = null;
            if(head is Constraint constraint)
            {
                t1Solutions = t2Solutions.Select(t2 =>
                {
                    var unified = Unify(t2, constraint);
                    if (unified == null)
                    {
                        return null;
                    }
                    return unified.Concat(t2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                });
            }
            else if(head is UnionConstraint union)
            {
                t1Solutions = t2Solutions.SelectMany(t2 =>
                {
                    var unified = Unify(t2, union);
                    if (unified == null)
                    {
                        return null;
                    }
                    return unified.Select(u => u.Concat(t2).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                });
            }

            /*
              
            string -> void
            char[] -> void

            int -> string
            int -> int
             */
            //var result = t1Solutions
            //    .Select(t1 => t1.Concat(t2Solutions).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
            var result = t1Solutions.Where(u => u != null);

            if (!t1Solutions.Any()) // no solution
                return null;

            //Console.WriteLine(Indent() + $"Unify returned: {{{string.Join("; ", result.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}}}");
            return result;
        }

        private static IEnumerable<IDictionary<string, IType>> Unify(IDictionary<string, IType> t2, UnionConstraint union)
        {
            foreach (var constraint in union.Constraints)
            {
                var result = UnifyOne(
                    Apply(t2, constraint.First),
                    Apply(t2, constraint.Second)
                );

                if(result != null)
                {
                    yield return result;
                }
            }
        }

        private static IDictionary<string, IType> Unify(IDictionary<string, IType> t2, Constraint constraint)
        {
            return UnifyOne(
                Apply(t2, constraint.First),
                Apply(t2, constraint.Second)
            );
        }

        //private static IDictionary<string, IType> ExpandUnionConstraint(IImmutableList<IConstraint> existing, UnionConstraint union)
        //{
        //    var results = union.potentialConstraints
        //        .SelectMany(argumentConstraints => Unify(argumentConstraints.Concat(existing).ToImmutableList()));

        //    return results
        //        .ToLookup(kvp => kvp.Key, kvp => kvp.Value);

        //}

        private static IDictionary<string, IType> UnifyOne(IType type1, IType type2)
        {
			Console.WriteLine(Indent() + $"UnifyOne: {type1}; {type2}");

            IDictionary<string, IType> result = null;
			if (type1 is ConcreteType c1 && type2 is ConcreteType c2 && c1.Equals(c2))
                result = new Dictionary<string, IType>();
            if (type1 is TypeVariable v1)
                result = new Dictionary<string, IType>
                {
                    { v1.Name, type2 }
                };
            if (type2 is TypeVariable v2)
                result = new Dictionary<string, IType>
                {
                    { v2.Name, type1 }
                };
            if (type1 is FunctionType f1 && type2 is FunctionType f2)
                result = Unify(
                    f1.ArgumentTypes
                        .Zip(f2.ArgumentTypes, (arg1, arg2) => new Constraint(arg1, arg2))
                        .Append(new Constraint(f1.ReturnType, f2.ReturnType))
                        .ToImmutableList<IConstraint>()
                )?.SingleOrDefault();

			//Console.WriteLine(Indent() + $"UnifyOne returned: {{{string.Join("; ", result.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}}}");
			return result;
        }

        private static IType Apply(IDictionary<string, IType> substitutions, IType targetType)
        {
			Console.WriteLine(Indent() + $"Apply: {{{string.Join("; ", substitutions.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}}} to {targetType}");

            var result = substitutions
                //.Reverse() // fold right, todo: needed? if so, make this more efficient
                .Aggregate(
                    targetType,
                    (t, kvp) => Substitute(t, kvp.Key, kvp.Value)
                );

			Console.WriteLine(Indent() + $"Apply returned: {result}");
			return result;
		}

        private static IType Substitute(IType targetType, string forType, IType withType)
        {
			Console.WriteLine(Indent() + $"Substitute {withType} for {forType} in target {targetType}");
            switch (targetType)
            {
                case ConcreteType concrete:
                    return targetType;
                case TypeVariable variable:
                    return variable.Name.Equals(forType)
                        ? withType
                        : targetType;
                case FunctionType function:
                    return new FunctionType(
                        function.ArgumentTypes.Select(arg => Substitute(arg, forType, withType)),
                        Substitute(function.ReturnType, forType, withType)
                    );
                default:
                    throw new ArgumentException("Unknown type " + targetType);
            }
        }

        public static ITypedExpression Apply(IDictionary<string, IType> substitutions, ITypedExpression expression)
        {
			Console.WriteLine(Indent() + $"Apply: {{{string.Join("; ", substitutions.Select(kvp => $"{kvp.Key}: {kvp.Value}"))}}} to {expression}");

            switch (expression)
            {
                case BooleanExpression booleanLiteral: return booleanLiteral;
                case LongExpression longLiteral: return longLiteral;
                case FloatExpression floatLiteral: return floatLiteral;
                case StringExpression stringLiteral: return stringLiteral;
                case DefTypeExpression defType: return defType;
                case FileExpression file:
                    return new FileExpression(file.Statements.Select(expr => Apply(substitutions, expr)));
                case DefExpression def:
                    return new DefExpression(
                        Apply(substitutions, def.Symbol) as SymbolExpression,
                        Apply(substitutions, def.Body),
                        Apply(substitutions, def.Type));
                case SymbolExpression symbol:
                    return new SymbolExpression(symbol.Name, Apply(substitutions, symbol.Type));
                case FunctionExpression function:
                    return new FunctionExpression(function.Symbol,
                        function.Arguments.Select(arg => Apply(substitutions, arg) as SymbolExpression).ToList(),
                        function.Body.Select(expr => Apply(substitutions, expr)).ToList(),
                        Apply(substitutions, function.Type));
                case ListExpression list:
                    return new ListExpression(
                        list.Elements.Select(el => Apply(substitutions, el)).ToList(),
                        Apply(substitutions, list.Type));
                case VectorExpression vector:
                    return new VectorExpression(
                        vector.Elements.Select(el => Apply(substitutions, el)).ToList(),
                        Apply(substitutions, vector.Type));
                case SetExpression set:
                    return new SetExpression(
                        set.Elements.Select(el => Apply(substitutions, el)).ToList(),
                        Apply(substitutions, set.Type));
                default:
                    throw new ArgumentException("unexpected type " + expression.GetType().Name);
            }
        }

		private static string Indent() =>
			new string(' ', Environment.StackTrace.Count(c => c == '\n'));
    }
}
