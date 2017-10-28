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

        public static void AddMacro(MethodDeclarationSyntax macro, string namespaceName, string className)
        {
            string methodName = macro.Identifier.ToString();

            var assemblies = new[]
            {
                typeof(SyntaxNode).GetTypeInfo().Assembly.Location,
                typeof(SyntaxFactory).GetTypeInfo().Assembly.Location,
            };

            var program = MacroCompilationUnit
                .AddMembers(NamespaceDeclaration(IdentifierName(namespaceName))
                    .AddMembers(ClassDeclaration(className)
                        .AddMembers(macro
                            .AddModifiers(new[] { Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword) }))));

            MacroStore[methodName] = new Lazy<MethodInfo>(() =>
            {
                var stream = Compiler.Compile(namespaceName, assemblies, OutputType.DynamicallyLinkedLibrary, program);
                var macroFunc = AssemblyRunner.GetFunction(stream, namespaceName, className, methodName);
                return macroFunc;
            });
        }

        internal static void ResolveMacro(string path)
        {
            var split = path.Split('.');
            var macroName = split.Last();
            var className = split.Take(split.Length - 1).StringJoin(".");
            MacroStore[macroName] = new Lazy<MethodInfo>(() =>
            {
                var containingClass = Type.GetType(className);
                //var containingClass = AppDomain.CurrentDomain.GetAssemblies()
                //    .Where(a => !a.IsDynamic)
                //    .SelectMany(a => a.GetTypes())
                //    .FirstOrDefault(t => t.FullName.Equals(className));
                var macro = containingClass.GetMethod(macroName, BindingFlags.Static | BindingFlags.Public);
                return macro;
            });
        }
    }
}
