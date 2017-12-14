using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

using Donatello.Ast;
using Donatello.Parser;
using Donatello.Emitter;
using Antlr4.Runtime;
using Donatello.Parser.Generated;

namespace Donatello
{
    class Program
    {
        public static void Main(String[] args)
        {
            var ast = AstProducer.Parse(@"(def a 5)");

            const string assemblyName = "Output";
            const string className = "Program";
            var assembly = BuildAssembly(ast, assemblyName, className);
            assembly.Save("Output.exe");
        }

        private static AssemblyBuilder BuildAssembly(FileUntypedExpression result, string assemblyName, string className)
        {
            string outputName = assemblyName + ".exe";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndSave);
            var module = assembly.DefineDynamicModule(outputName, outputName, true);

            // process our AST and emit IL for a single class
            //new ClassEmitter(assembly, module, className).Visit(result);

            return assembly;
        }
    }
}
