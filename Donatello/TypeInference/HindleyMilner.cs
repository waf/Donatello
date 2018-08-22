using Donatello.Ast;

namespace Donatello.TypeInference
{
    static class HindleyMilner
    {
        public static ITypedExpression Infer(IExpression tree)
        {
            // annotate the tree with type variables. e.g. 'a 'b 'c
            var annotated = Annotator.Annotate(tree);
            // gather the constraints between these type variables
            var constraints = ConstraintCollector.Collect(annotated);
            // find the most general unifier of these constraints
            var unified = TypeUnifier.Unify(constraints);
            // apply constraints to tree with the type variables
            var typed = TypeUnifier.Apply(unified, annotated);
            return typed;
        }
    }
}
