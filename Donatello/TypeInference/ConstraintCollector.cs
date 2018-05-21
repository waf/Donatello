using Donatello.Ast;
using Donatello.Services;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.TypeInference
{
    class Constraint
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

    internal class ConstraintCollector : AstOperation<IImmutableList<Constraint>>
    {
        private static readonly IImmutableList<Constraint> NoConstraints = ImmutableList<Constraint>.Empty;

        internal static IImmutableList<Constraint> Collect(ITypedExpression annotated)
        {
            var collector = new ConstraintCollector();
            return collector.Apply(annotated);
        }

        // no constraints on literals
        protected override IImmutableList<Constraint> FloatLiteral(FloatExpression expr) => NoConstraints;
        protected override IImmutableList<Constraint> LongLiteral(LongExpression expr) => NoConstraints;
        protected override IImmutableList<Constraint> BooleanLiteral(BooleanExpression expr) => NoConstraints;
        protected override IImmutableList<Constraint> StringLiteral(StringExpression expr) => NoConstraints;
        protected override IImmutableList<Constraint> DefType(DefTypeExpression defType) => NoConstraints;

        // a single symbol gives us no information
        protected override IImmutableList<Constraint> Symbol(SymbolExpression expr) => NoConstraints;

        protected override IImmutableList<Constraint> List(ListExpression list)
        {
            var fn = list.Elements.First();
            var args = list.Elements.Skip(1);
            switch (fn.Type)
            {
                case FunctionType fnType:
                    return Apply(fn)
                        .Concat(args.SelectMany(Apply))
                        .Concat(
                            // each argument in the list is the same as the function's argument
                            args.Zip(fnType.ArgumentTypes, (arg, fnArgType) => new Constraint(arg.Type, fnArgType))
                        )
                        .Concat(new[] {
                            // the expression type is the same as the function's return type
                            new Constraint(list.Type, fnType.ReturnType)
                        })
                        .ToImmutableList();
                case TypeVariable varType:
                    return Apply(fn)
                        .Concat(args.SelectMany(Apply))
                        .Concat(new[]
                        {
                            // the type of the function is the same as (types of the arguments) -> type of list
                            new Constraint(fn.Type, new FunctionType(args.Select(a => a.Type), list.Type))
                        })
                        .ToImmutableList();
                default:
                    throw new ArgumentException("Unexpected type found in constraint resolution");
            }
        }

        protected override IImmutableList<Constraint> Vector(VectorExpression expr)
        {
            throw new NotImplementedException();
        }

        protected override IImmutableList<Constraint> Set(SetExpression expr)
        {
            throw new NotImplementedException();
        }

        protected override IImmutableList<Constraint> Def(DefExpression expr)
        {
            return ImmutableList.Create(
                new Constraint(expr.Symbol.Type, expr.Body.Type)
            );
        }

        protected override IImmutableList<Constraint> Function(FunctionExpression expr)
        {
            return ImmutableList.Create(
                // the function's return type is the same as the type of the last statement in the body.
                new Constraint(expr.Type.ReturnType, expr.Body.Last().Type),
                new Constraint(expr.Symbol.Type, new FunctionType(expr.Arguments.Select(arg => arg.Type), expr.Body.Last().Type))
            );
        }

        protected override IImmutableList<Constraint> File(FileExpression expr)
        {
            return expr.Statements.SelectMany(Apply).ToImmutableList();
        }
    }
}
