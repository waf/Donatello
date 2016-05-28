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

namespace DotNetLisp.Compilation
{
    public static class Compiler
    {
        [Conditional("DEBUG")]
        public static void TranslateToCSharp(CSharpSyntaxNode programExpression)
        {
            var cw = new AdhocWorkspace();
            cw.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            string formatted = Formatter.Format(programExpression, cw).ToString();
            return;
        }

        public static Result<byte[], string[]> Compile(string dllName, params CompilationUnitSyntax[] programs)
        {
            var references = programs
                .SelectMany(program => AddDefaultReferences(ref program))
                .Distinct()
                .ToList();
            var trees = programs.Select(program => CSharpSyntaxTree.Create(program));

            CSharpCompilation compilation = CSharpCompilation.Create(
                dllName,
                syntaxTrees: trees,
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return CreateAssembly(compilation);
        }

        /// <summary>
        /// Compile the roslyn AST
        /// </summary>
        /// <returns>Either the compiled bytes of the program, or a list of error messages</returns>
        private static Result<byte[], string[]> CreateAssembly(CSharpCompilation compilation)
        {
            using (var ms = new MemoryStream())
            {
                EmitResult emmitted = compilation.Emit(ms);

                if (!emmitted.Success)
                {
                    // create error messages
                    return emmitted.Diagnostics
                        .Where(diagnostic =>
                            diagnostic.IsWarningAsError ||
                            diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}")
                        .ToArray();
                }

                // load the program and run it
                ms.Seek(0, SeekOrigin.Begin);
                return ms.ToArray();
            }
        }

        private static MetadataReference[] AddDefaultReferences(ref CompilationUnitSyntax program)
        {
            var defaultImports = new[]
            {
                // TODO: maybe this project should be a PCL so we can reference our own System.Object?
                //new { Namespace = "System", DllFile = @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETPortable\v4.6\Profile\Profile151\System.Runtime.dll" }, // we can't use our own system.object here, since it won't work if we reference PCLs
                new { Namespace = "System", DllFile = typeof(object).Assembly.Location },
                new { Namespace = "System.Linq", DllFile = typeof(Enumerable).Assembly.Location },
                new { Namespace = "System.Collections.Immutable", DllFile = typeof(ImmutableArray).Assembly.Location },
                new { Namespace = "DotNetLisp.StandardLibrary", DllFile = typeof(Constructors).Assembly.Location },
            };

            // add facade references for PCL support (like immutable collections)
            var facades = Directory.GetFiles(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6\Facades", "*.dll")
                .Select(file => MetadataReference.CreateFromFile(file))
                .ToArray();

            var usings = defaultImports
                .Select(import => CreateUsingDirective(import.Namespace))
                .ToArray();
            program = program.AddUsings(usings);

            TranslateToCSharp(program); // for debugging purposes

            MetadataReference[] references = defaultImports
                .Select(import => MetadataReference.CreateFromFile(import.DllFile))
                .Union(facades)
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
