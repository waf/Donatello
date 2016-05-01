using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using DotNetLisp.Antlr.Generated;
using DotNetLisp.Compilation;
using DotNetLisp.Parser;
using DotNetLisp.Util;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

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
            // there's a slight delay when we load up roslyn and run a program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience the delay for the first evaluation.
            Task.Run(() => Compiler.Compile(LiteralExpression(SyntaxKind.StringLiteralExpression)));

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
                    var ast = Evaluate(input);
                    var assembly = Compiler.Compile(ast);
                    // either run the compiled output or handle the errors
                    output = assembly.Match(
                        Ok: bytes => FormatProgramOutput(DynamicInvoke(bytes)),
                        Error: errors => string.Join(Environment.NewLine, errors));
                }
                catch (Exception e)
                {
                    output = "Error: " + e.Message;
                }

                // print
                Console.WriteLine(output);
            } // loop!
        }

        private static string FormatProgramOutput(object output)
        {
            return JsonConvert.SerializeObject(output, Formatting.Indented);
        }

        private static object DynamicInvoke(byte[] bytes)
        {
            Assembly assembly = Assembly.Load(bytes);
            Type type = assembly.GetType("Repl.Program");
            var result = type.InvokeMember("Run", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
            return result;
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
    }
}
