using Donatello.Ast;
using Donatello.Emitter;
using System.Reflection;
using System.Reflection.Emit;

namespace Donatello
{
    static class Compiler
    {
        public static AssemblyBuilder BuildAssembly(ITypedExpression result, string assemblyName, string className)
        {
            string moduleName = assemblyName + ".exe";
            var assembly = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.RunAndSave);
            var module = assembly.DefineDynamicModule(moduleName, moduleName, true);

            // process our AST and emit IL for a single class
            new ClassEmitter(assembly, module, className).Visit(result);

            return assembly;
        }
    }
}
