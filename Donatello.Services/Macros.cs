using Antlr4.Runtime.Tree;
using Donatello.Services.Compilation;
using Donatello.Services.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services
{
    static class Macros
    {
        static readonly IDictionary<string, Lazy<MethodInfo>> MacroStore = new Dictionary<string, Lazy<MethodInfo>>();
        static readonly CompilationUnitSyntax MacroCompilationUnit;

        static Macros ()
        {
            MacroCompilationUnit = CompilationUnit()
                .WithUsings(List(new[]
                {
                    //TODO: hard-coding references isn't great. can we get them from the file that defines the macro?
                    UsingDirective(ParseName("Microsoft.CodeAnalysis.CSharp")),
                    UsingDirective(ParseName("Microsoft.CodeAnalysis.CSharp.Syntax")),
                    UsingDirective(Token(SyntaxKind.StaticKeyword), null,
                        ParseName("Microsoft.CodeAnalysis.CSharp.SyntaxFactory")),
                }));
        }

        public static bool TryGetMacro(string name, out Lazy<MethodInfo> macro) =>
            MacroStore.TryGetValue(name, out macro);

        public static bool TryRunMacro(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            string name, IList<IParseTree> input, out CSharpSyntaxNode output)
        {
            if (!MacroStore.TryGetValue(name, out var lazyMacro))
            {
                output = null;
                return false;
            }
            var arguments = input.Select(visitor.Visit);
            output = lazyMacro
                .Value
                .Invoke(null, arguments.ToArray())
                as CSharpSyntaxNode;
            return true;
        }

        public static void AddMacro(MethodDeclarationSyntax macro)
        {
            string methodName = macro.Identifier.ToString();
            string className = methodName + "Macro";
            string assemblyName = className + "Assembly";
            string namespaceName = assemblyName;

            var assemblies = new[]
            {
                typeof(SyntaxNode).Assembly.Location,
                typeof(SyntaxFactory).Assembly.Location,
            };

            var program = MacroCompilationUnit
                .AddMembers(NamespaceDeclaration(IdentifierName(namespaceName))
                    .AddMembers(ClassDeclaration(className)
                        .AddMembers(macro
                            .AddModifiers(new[] { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) }))));

            MacroStore[methodName] = new Lazy<MethodInfo>(() =>
            {
                var bytes = Compiler.Compile(assemblyName, assemblies, OutputType.DynamicallyLinkedLibrary, program);
                var macroFunc = AssemblyRunner.GetFunction(bytes, namespaceName, className, methodName);
                return macroFunc;
            });
        }
    }
}
