using Antlr4.Runtime.Tree;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello
{
    using Compilation;
    using Microsoft.CodeAnalysis.CSharp;
    using System.Reflection;
    using Util;

    static class Macros
    {
        static readonly IDictionary<string, Lazy<MethodInfo>> MacroStore = new Dictionary<string, Lazy<MethodInfo>>();

        public static bool TryGetMacro(string name, out Lazy<MethodInfo> macro) =>
            MacroStore.TryGetValue(name, out macro);

        public static bool TryRunMacro(
            string name, IList<IParseTree> input, out IParseTree output)
        {
            Lazy<MethodInfo> lazyMacro;
            if(!MacroStore.TryGetValue(name, out lazyMacro))
            {
                output = null;
                return false;
            }
            output = lazyMacro
                .Value
                .Invoke(null, input.ToArray())
                as IParseTree;
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

            MacroStore[methodName] = new Lazy<MethodInfo>(() =>
            {
                var bytes = Compiler.Compile(assemblyName, new string[0], OutputType.DynamicallyLinkedLibrary, program);
                var macroFunc = AssemblyRunner.GetFunction(bytes, namespaceName, className, methodName);
                return macroFunc;
            });
        }
    }
}
