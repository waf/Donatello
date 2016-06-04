using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using DotNetLisp.Util;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics;
using System.Collections.Immutable;
using DotNetLisp.StandardLibrary;
using System.Reflection;

namespace DotNetLisp.Compilation
{
    public enum OutputType
    {
        ConsoleApplication = OutputKind.ConsoleApplication,
        WindowsApplication = OutputKind.WindowsApplication,
        DynamicallyLinkedLibrary = OutputKind.DynamicallyLinkedLibrary,
    }
    public static class Compiler
    {
        static readonly IDictionary<string, Assembly> DefaultImports = new Dictionary<string, Assembly>
        {
            { "System", typeof(object).Assembly },
            { "System.Linq", typeof(Enumerable).Assembly },
            { "System.Collections.Immutable", typeof(ImmutableArray).Assembly },
            { "DotNetLisp.StandardLibrary", typeof(Constructors).Assembly }
        };

        [Conditional("DEBUG")]
        public static void TranslateToCSharp(CSharpSyntaxNode programExpression)
        {
            var cw = new AdhocWorkspace();
            cw.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            string formatted = Formatter.Format(programExpression, cw).ToString();
            return;
        }

        public static byte[] Compile(string assemblyName, OutputType outputKind, params CompilationUnitSyntax[] programs)
        {
            var trees = programs.Select(program =>
            {
                var defaultUsings = DefaultImports.Select(import => CreateUsingDirective(import.Key)).ToArray();
                program = program.AddUsings(defaultUsings);
                TranslateToCSharp(program);
                return CSharpSyntaxTree.Create(program);
            }).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: trees,
                references: GetDefaultReferences(),
                options: new CSharpCompilationOptions((OutputKind)outputKind));

            return CreateAssembly(compilation);
        }

        /// <summary>
        /// Compile the roslyn AST
        /// </summary>
        /// <returns>Either the compiled bytes of the program, or a list of error messages</returns>
        private static byte[] CreateAssembly(CSharpCompilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult emmitted = compilation.Emit(ms);

                if (!emmitted.Success)
                {
                    // create error messages
                    var errors = emmitted.Diagnostics
                        .Where(diagnostic =>
                            diagnostic.IsWarningAsError ||
                            diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}");

                    throw new Exception(string.Join(Environment.NewLine, errors));
                }

                // load the program and run it
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        private static MetadataReference[] GetDefaultReferences()
        {
            // add facade references for PCL support (like immutable collections)
            var facades = Directory.GetFiles(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\Facades", "*.dll")
                .Select(file => MetadataReference.CreateFromFile(file))
                .ToArray();

            MetadataReference[] references = DefaultImports
                .Select(import => MetadataReference.CreateFromFile(import.Value.Location))
                .Union(facades)
                .Distinct()
                .ToArray();

            return references;
        }

        private static UsingDirectiveSyntax CreateUsingDirective(string usingName)
        {
            //TODO: stole this method from the internet. can it be better?
            NameSyntax qualifiedName = null;

            foreach (var identifier in usingName.Split('.'))
            {
                var name = IdentifierName(identifier);

                if (qualifiedName != null)
                {
                    qualifiedName = QualifiedName(qualifiedName, name);
                }
                else
                {
                    qualifiedName = name;
                }
            }

            return UsingDirective(qualifiedName);
        }
    }
}
