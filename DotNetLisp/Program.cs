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
using System.Reflection;

namespace DotNetLisp
{
    class Program
    {
        public static ParseTreeProperty<Scope> ScopeAnnotations = new ParseTreeProperty<Scope>();
        public static Scope GlobalScope = new Scope();

        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  DotNetLisp           open a repl");
                Console.WriteLine("  DotNetLisp <file>    compile a file");
            }

            BuiltInFunctions.AddBuiltinFunctions(GlobalScope);

            // compile file
            if (args.Length == 2)
            {
                //CompileToDll(args);
                return;
            }

            ReadEvalPrintLoop();
        }

        private static void ReadEvalPrintLoop()
        {
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
                    var programExpression = Evaluate(input);

                    var unit = SyntaxFactory.CompilationUnit()
                        .AddMembers(SyntaxFactory.NamespaceDeclaration(SyntaxFactory.IdentifierName("Repl"))
                                .AddMembers(SyntaxFactory.ClassDeclaration("Program")
                                        .AddMembers(SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName("System.Object"), "Run")
                                            .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword))
                                            .WithBody(SyntaxFactory.Block(SyntaxFactory.ReturnStatement(programExpression))))));


                    var assemblyPath = Path.GetDirectoryName(typeof(object).Assembly.Location);
                    MetadataReference[] references = new MetadataReference[]
                    {
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location)
                    };

                    CSharpCompilation compilation = CSharpCompilation.Create(
                        Path.GetRandomFileName(),
                        syntaxTrees: new[] { CSharpSyntaxTree.Create(unit) },
                        references: references,
                        options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                    
                    using (var ms = new MemoryStream())
                    {
                        EmitResult emmitted = compilation.Emit(ms);

                        if (!emmitted.Success)
                        {
                            IEnumerable<Diagnostic> failures = emmitted.Diagnostics.Where(diagnostic =>
                                diagnostic.IsWarningAsError ||
                                diagnostic.Severity == DiagnosticSeverity.Error);
                            foreach (Diagnostic diagnostic in failures)
                            {
                                Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                            }
                        }
                        else
                        {
                            ms.Seek(0, SeekOrigin.Begin);
                            Assembly assembly = Assembly.Load(ms.ToArray());
                            Type type = assembly.GetType("Repl.Program");
                            var result = type.InvokeMember("Run", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
                            output = JsonConvert.SerializeObject(result, Formatting.Indented);
                            //TODO: unload assembly
                        }
                    }
                }
                catch (Exception e)
                {
                    output = "Error: " + e.Message;
                }

                // print
                Console.WriteLine(output);

            } // loop!
        }

        /*
        private static void CompileToDll(string[] args)
        {
            var file = File.ReadAllText(args[1]);

            var programExpression = Evaluate(file);
            var param = Expression.Parameter(typeof(string));
            var mainMethod = Expression.Lambda<Action<string[]>>(programExpression, param);
            var compiled = mainMethod.Compile();
            //todo: create a program with the compiled method as the 'Main' method, output DLL.
        }
        */

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
    }
}
