using Donatello.Ast;
using Donatello.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Donatello.TypeInference
{
	class Constraint : IConstraint
    {
        public Constraint(IType first, IType second)
        {
            First = first;
            Second = second;
        }

        public IType First { get; }
        public IType Second { get; }

        public override string ToString() => $"{First}, {Second}";
    }

    internal class UnionConstraint : IConstraint
    {
        public IReadOnlyCollection<Constraint> Constraints { get; }

        public UnionConstraint(IReadOnlyCollection<Constraint> constraints)
        {
            this.Constraints = constraints;
        }
    }

    interface IConstraint
	{

	}

    internal class ConstraintCollector : AstOperation<IImmutableList<IConstraint>>
    {
        private static readonly IImmutableList<IConstraint> NoConstraints = ImmutableList<IConstraint>.Empty;

        internal static IImmutableList<IConstraint> Collect(ITypedExpression annotated)
        {
            var collector = new ConstraintCollector();
            return collector.Apply(annotated);
        }

        // no constraints on literals
        protected override IImmutableList<IConstraint> FloatLiteral(FloatExpression expr) => NoConstraints;
        protected override IImmutableList<IConstraint> LongLiteral(LongExpression expr) => NoConstraints;
        protected override IImmutableList<IConstraint> BooleanLiteral(BooleanExpression expr) => NoConstraints;
        protected override IImmutableList<IConstraint> StringLiteral(StringExpression expr) => NoConstraints;
        protected override IImmutableList<IConstraint> DefType(DefTypeExpression defType) => NoConstraints;

		// a single symbol gives us no information
		protected override IImmutableList<IConstraint> Symbol(SymbolExpression expr)
		{
			return NoConstraints;
		}

        protected override IImmutableList<IConstraint> List(ListExpression list)
        {
            var fn = list.Elements.First();
            var args = list.Elements.Skip(1).ToList();

            var constraints = Apply(fn)
                .Concat(args.SelectMany(Apply))
                .Append(
                    // the type of the function is the same as (types of the arguments) -> type of list
                    new Constraint(fn.Type, new FunctionType(args.Select(a => a.Type), list.Type))
                );

            switch (fn.Type)
            {
                case TypeVariable fnType when fn is SymbolExpression symbol:
                    return constraints
                        .Concat(GetConstraintsForExternalFunction(symbol, fn.Type, args, list.Type))
                        .ToImmutableList();
                case TypeVariable varType:
                    return constraints
                        .ToImmutableList();
                default:
                    throw new ArgumentException("Unexpected type found in constraint resolution");
            }
        }

        private static IEnumerable<IConstraint> GetConstraintsForExternalFunction(
            SymbolExpression symbol, IType functionType, IReadOnlyCollection<ITypedExpression> args, IType returnType)
        {
            int endOfType = symbol.Name.LastIndexOf('.');

            if(endOfType == -1)
            {
                yield break;
            }

            var externalFunctions = Type
                .GetType(symbol.Name.Substring(0, endOfType), false, false)
                ?.GetMethods()
                .Where(m => m.Name == symbol.Name.Substring(endOfType + 1)
                            && m.GetParameters().Length == args.Count)
                .ToList();

            // if we found a .NET function, add constraints for the arguments and return types
            if (externalFunctions.Count == 0)
            {
                yield break;
            }
            else if (externalFunctions.Count == 1)
            {
                yield return CreateConstraintForFunction(externalFunctions[0]);
            }
            else
            {
                yield return new UnionConstraint(externalFunctions.Select(CreateConstraintForFunction).ToList());
            }

            Constraint CreateConstraintForFunction(MethodInfo dotNetFunction) =>
                new Constraint(functionType, new FunctionType (
                    argTypes: dotNetFunction.GetParameters().Select(param => new ConcreteType(param.ParameterType)),
                    returnType: new ConcreteType(dotNetFunction.ReturnType)
                ));
        }

        protected override IImmutableList<IConstraint> Vector(VectorExpression expr)
        {
            throw new NotImplementedException();
        }

        protected override IImmutableList<IConstraint> Set(SetExpression expr)
        {
            throw new NotImplementedException();
        }

        protected override IImmutableList<IConstraint> Def(DefExpression expr)
        {
            return ImmutableList.Create<IConstraint>(
                new Constraint(expr.Symbol.Type, expr.Body.Type)
            );
        }

        protected override IImmutableList<IConstraint> Function(FunctionExpression expr)
        {
            return ImmutableList.Create<IConstraint>(
                // the function's return type is the same as the type of the last statement in the body.
                new Constraint(expr.Type.ReturnType, expr.Body.Last().Type),
                new Constraint(expr.Symbol.Type, new FunctionType(expr.Arguments.Select(arg => arg.Type), expr.Body.Last().Type))
            );
        }

        protected override IImmutableList<IConstraint> File(FileExpression expr)
        {
            return expr.Statements.SelectMany(Apply).ToImmutableList();
        }
    }
}
