using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.Emit;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;
using System.Diagnostics;
using System.Collections.Immutable;
using Donatello.StandardLibrary;
using System.Reflection;

namespace Donatello.Services.Compilation
{
    public enum OutputType
    {
        ConsoleApplication = OutputKind.ConsoleApplication,
        WindowsApplication = OutputKind.WindowsApplication,
        DynamicallyLinkedLibrary = OutputKind.DynamicallyLinkedLibrary,
    }
    public static class Compiler
    {
        public static readonly IDictionary<string, Assembly> DefaultImports = new Dictionary<string, Assembly>
        {
            { "System", typeof(object).GetTypeInfo().Assembly },
            { "System.Linq", typeof(Enumerable).GetTypeInfo().Assembly },
            { "System.Collections.Immutable", typeof(ImmutableArray).GetTypeInfo().Assembly },
            { "System.Collections.Generic", typeof(IEnumerable<>).GetTypeInfo().Assembly },
            { "Donatello.StandardLibrary", typeof(Constructors).GetTypeInfo().Assembly }
        };
        public static Lazy<IReadOnlyCollection<string>> DotNetCoreAssemblies = 
            new Lazy<IReadOnlyCollection<string>>(GetDotNetCoreAssemblies);

        /// <summary>
        /// Helpful method while debugging, see the translated C# source.
        /// </summary>
        [Conditional("DEBUG")]
        public static void TranslateToCSharp(CSharpSyntaxNode programExpression)
        {
            var cw = new AdhocWorkspace();
            cw.Options.WithChangedOption(CSharpFormattingOptions.IndentBraces, true);
            string formatted = Formatter.Format(programExpression, cw).ToString();
            return;
        }

        public static Stream Compile(string assemblyName,
            IReadOnlyCollection<string> references,
            OutputType outputKind,
            params CompilationUnitSyntax[] programs)
        {
            var defaultUsings = DefaultImports.Select(import => UsingDirective(ParseName(import.Key))).ToArray();
            var trees = programs.Select(program =>
            {
                program = program.AddUsings(defaultUsings);
                TranslateToCSharp(program);
                return CSharpSyntaxTree.Create(program);
            }).ToArray();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: trees,
                references: GetDefaultReferences(references.ToArray()),
                options: new CSharpCompilationOptions((OutputKind)outputKind));

            return CreateAssembly(compilation);
        }

        /// <summary>
        /// Compile the roslyn AST
        /// </summary>
        /// <returns>Either the compiled bytes of the program, or a list of error messages</returns>
        private static Stream CreateAssembly(CSharpCompilation compilation)
        {
            var ms = new MemoryStream();
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
            return ms;
        }

        public static MetadataReference[] GetDefaultReferences(params string[] additionalReferences)
        {
            MetadataReference[] allReferences = DefaultImports
                .Select(import => import.Value.Location)
                .Union(DotNetCoreAssemblies.Value)
                .Union(additionalReferences)
                .Distinct()
                .Select(dll => MetadataReference.CreateFromFile(dll.Trim()))
                .ToArray();

            return allReferences;
        }

        private static IReadOnlyCollection<string> GetDotNetCoreAssemblies()
        {
            // https://github.com/dotnet/roslyn/wiki/Runtime-code-generation-using-Roslyn-compilations-in-.NET-Core-App
            return Directory
                .GetParent(
                    AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
                        .ToString().Split(';')
                        .Single(path => path.EndsWith("netstandard.dll"))
                )
                .GetFiles("*.dll")
                .Where(file => file.Name.StartsWith("System") || file.Name == "netstandard.dll")
                .Select(file => file.FullName)
                .ToArray();
        }
    }
}
