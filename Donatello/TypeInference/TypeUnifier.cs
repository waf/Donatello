using Donatello.Ast;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.TypeInference
{
    internal class TypeUnifier
    {
        private static IType Substitute(IType targetType, string forType, IType withType)
        {
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

        public static IType Apply(IDictionary<string, IType> substitutions, IType targetType)
        {
            return substitutions
                .Reverse() // fold right, todo: needed? if so, make this more efficient
                .Aggregate(
                    targetType,
                    (t, kvp) => Substitute(t, kvp.Key, kvp.Value)
                );
        }

        public static IDictionary<string, IType> Unify(IImmutableList<Constraint> constraints)
        {
            if (!constraints.Any())
                return new Dictionary<string, IType>();

            var head = constraints.First();
            var tail = constraints.Skip(1).ToImmutableList();
            var t2 = Unify(tail);
            var t1 = UnifyOne(
                Apply(t2, head.First),
                Apply(t2, head.Second)
            );
            return t1.Concat(t2)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        private static IDictionary<string, IType> UnifyOne(IType type1, IType type2)
        {
            if (type1 is ConcreteType c1 && type2 is ConcreteType c2 && c1.Equals(c2))
                return new Dictionary<string, IType>();
            if (type1 is TypeVariable v1)
                return new Dictionary<string, IType>
                {
                    { v1.Name, type2 }
                };
            if (type2 is TypeVariable v2)
                return new Dictionary<string, IType>
                {
                    { v2.Name, type1 }
                };
            if (type1 is FunctionType f1 && type2 is FunctionType f2)
                return Unify(
                    f1.ArgumentTypes
                        .Zip(f2.ArgumentTypes, (arg1, arg2) => new Constraint(arg1, arg2))
                        .Append(new Constraint(f1.ReturnType, f2.ReturnType))
                        .ToImmutableList()
                );

            throw new InvalidOperationException("mismatched types");
        }

        public static ITypedExpression Apply(IDictionary<string, IType> substitutions, ITypedExpression expression)
        {
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
    }
}
