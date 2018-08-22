using System;
using System.Linq;
using Donatello.Parser;
using Donatello.TypeInference;

namespace Donatello
{
    static class Program
    {
        const string assemblyName = "Output";
        const string className = "Program";

        public static void Main(String[] args)
        {
            var ast = AstProducer.Parse("(add 5 3)");
            var tast = Annotator.Annotate(ast);
            var constraints = ConstraintCollector.Collect(tast);
            var unified = TypeUnifier.Unify(constraints);
            var typed = TypeUnifier.Apply(unified, tast);
			return;

            //var ast = AstProducer.Parse(@"(deftype Cat -color String -name String)");
            //var typedAst = HindleyMilner.Infer(ast);
            //var assembly = Compiler.BuildAssembly(typedAst, assemblyName, className);

            //string outputName = assembly.Modules.Last().ScopeName;
            //assembly.Save(outputName);
        }
    }
}
