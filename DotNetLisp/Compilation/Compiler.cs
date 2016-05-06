using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;
using DotNetLisp.Util;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics;

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

        public static Result<byte[], string[]> Compile(CSharpSyntaxNode programExpression)
        {
            MetadataReference[] references = {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: new[] { CSharpSyntaxTree.Create(programExpression as CompilationUnitSyntax) },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            return CreateAssembly(compilation);
        }

        public static Result<byte[], string[]> CompileForRepl(CSharpSyntaxNode programExpression)
        {
            MetadataReference[] references = {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.CreateScriptCompilation(
                Path.GetRandomFileName(),
                syntaxTree: CSharpSyntaxTree.Create(programExpression as CompilationUnitSyntax, new CSharpParseOptions(kind: SourceCodeKind.Script)),
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
    }
}
