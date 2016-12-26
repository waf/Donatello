using Antlr4.Runtime.Tree;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using MacroArgument = System.Collections.Immutable.IImmutableList<Antlr4.Runtime.Tree.IParseTree>;
using MacroReturn = System.Collections.Generic.IEnumerable<Antlr4.Runtime.Tree.IParseTree>;

namespace Donatello
{
    using Compilation;
    using Microsoft.CodeAnalysis.CSharp;
    using Util;
    using MacroFunc = Func<MacroArgument, MacroReturn>;

    static class Macros
    {
        static readonly IDictionary<string, Lazy<MacroFunc>> MacroStore = new Dictionary<string, Lazy<MacroFunc>>();

        public static bool TryGetMacro(string name, out Lazy<MacroFunc> macro) =>
            MacroStore.TryGetValue(name, out macro);

        public static bool TryRunMacro(
            string name, IList<IParseTree> input, out IList<IParseTree> output)
        {
            Lazy<MacroFunc> lazyMacro;
            if(!MacroStore.TryGetValue(name, out lazyMacro))
            {
                output = null;
                return false;
            }
            output = lazyMacro
                .Value(input.ToImmutableList())
                .ToList();
            return true;
        }

        public static void AddMacro(MethodDeclarationSyntax macro)
        {
            string methodName = macro.Identifier.ToString();
            string className = methodName + "Macro";
            string assemblyName = className + "Assembly";
            string namespaceName = assemblyName;

            var program = CompilationUnit()
                .AddMembers(NamespaceDeclaration(IdentifierName(namespaceName))
                    .AddMembers(ClassDeclaration(className)
                        .AddMembers(macro
                            .AddModifiers(new[] { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) }))));

            MacroStore[methodName] = new Lazy<MacroFunc>(() =>
            {
                var bytes = Compiler.Compile(assemblyName, new string[0], OutputType.DynamicallyLinkedLibrary, program);
                var macroFunc = AssemblyRunner.GetFunction<MacroArgument, MacroReturn>(bytes, namespaceName, className, methodName);
                return macroFunc;
            });
        }
    }
}
