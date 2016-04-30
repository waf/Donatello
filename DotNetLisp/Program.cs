using Antlr4.Runtime;
using DotNetLisp.Antlr.Generated;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DotNetLisp.Parser;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Tree;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Reflection;

namespace DotNetLisp
{
    static class Program
    {
        internal readonly static ParseTreeProperty<Scope> ScopeAnnotations = new ParseTreeProperty<Scope>();
        internal readonly static Scope GlobalScope = new Scope();

        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  DotNetLisp           open a repl");
                Console.WriteLine("  DotNetLisp <file>    compile a file");
            }

            BuiltInFunctions.AddBuiltInVariables(GlobalScope);

            ReadEvalPrintLoop();
        }

        private static void ReadEvalPrintLoop()
        {
            // there's a slight delay when we load up roslyn and run the program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience the delay for the first evaluation.
            Task.Run(() => CompileAndRun(LiteralExpression(SyntaxKind.StringLiteralExpression)));

            while (true)
            {
                //read
                Console.Write("> ");
                string input = Console.ReadLine().Trim();

                if (input == string.Empty) { continue; }
                if (input == "exit") { break; }

                //eval
                string output = "";
                try
                {
                    var program = Evaluate(input);
                    output = CompileAndRun(program);
                }
                catch (Exception e)
                {
                    output = "Error: " + e.Message;
                }

                // print
                Console.WriteLine(output);
            } // loop!
        }

        /// <summary>
        /// Use ANTLR4 and the associated visitor implementation to produce a roslyn AST
        /// </summary>
        private static ExpressionSyntax Evaluate(string input)
        {
            var visitor = new ParseExpressionVisitor();

            using (var stream = new StringReader(input))
            {
                AntlrInputStream inputStream = new AntlrInputStream(stream);

                DotNetLispLexer lexer = new DotNetLispLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                DotNetLispParser parser = new DotNetLispParser(commonTokenStream);
                var file = parser.file();

                return visitor.Visit(file);
            }
        }

        /// <summary>
        /// Compile and run the roslyn AST
        /// </summary>
        /// <returns>The output of the program</returns>
        private static string CompileAndRun(ExpressionSyntax programExpression)
        {
            // make a Program class that has a "Run" method, and embed our program expression inside it.
            var program = CompilationUnit()
                .AddMembers(NamespaceDeclaration(IdentifierName("Repl"))
                    .AddMembers(ClassDeclaration("Program")
                        .AddMembers(MethodDeclaration(ParseTypeName("System.Object"), "Run")
                           .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                           .WithBody(Block(ReturnStatement(programExpression))))));

            MetadataReference[] references = {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: new[] { CSharpSyntaxTree.Create(program) },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (var ms = new MemoryStream())
            {
                EmitResult emmitted = compilation.Emit(ms);

                if (!emmitted.Success)
                {
                    // create error messages
                    return string.Join(Environment.NewLine, emmitted.Diagnostics
                        .Where(diagnostic =>
                            diagnostic.IsWarningAsError ||
                            diagnostic.Severity == DiagnosticSeverity.Error)
                        .Select(diagnostic => $"{diagnostic.Id}: {diagnostic.GetMessage()}"));
                }

                // load the program and run it
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());
                Type type = assembly.GetType("Repl.Program");
                var result = type.InvokeMember("Run", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
                return JsonConvert.SerializeObject(result, Formatting.Indented);
            }
        }
    }
}
