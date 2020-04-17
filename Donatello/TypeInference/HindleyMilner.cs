using Donatello.Ast;

namespace Donatello.TypeInference
{
    static class HindleyMilner
    {
        public static ITypedExpression Infer(IExpression tree)
        {
            // annotate the tree with type variables. e.g. 'a 'b 'c
            var annotatedTree = Annotator.Annotate(tree);
            // gather the constraints between these type variables
            var typeConstraints = ConstraintCollector.Collect(annotatedTree);
            // find the most general unifier of these constraints
            var unifiedConstraints = TypeUnifier.UnifyAll(typeConstraints);
            // apply constraints to tree with the type variables
            var typedTree = TypeUnifier.Apply(unifiedConstraints, annotatedTree);
            return typedTree;
        }
    }
}
