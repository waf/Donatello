using System;
using System.Linq;
using Donatello.Parser;
using Donatello.TypeInference;

namespace Donatello
{
    class Program
    {
        const string assemblyName = "Output";
        const string className = "Program";

        public static void Main(String[] args)
        {
            var ast = AstProducer.Parse(@"(deftype Cat -color String -name String)");
            var typedAst = HindleyMilner.Infer(ast);
            var assembly = Compiler.BuildAssembly(typedAst, assemblyName, className);

            string outputName = assembly.Modules.Last().ScopeName;
            assembly.Save(outputName);
        }
    }
}
