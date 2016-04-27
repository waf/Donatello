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

            // compile file
            if (args.Length == 2)
            {
                CompileToDll(args);
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
                string output;
                try
                {
                    Expression programExpression = Evaluate(input);
                    var wrapped = Expression.Convert(programExpression, typeof(object));
                    var mainMethod = Expression.Lambda<Func<object>>(wrapped);
                    var result = mainMethod.Compile().Invoke();
                    output = JsonConvert.SerializeObject(result, Formatting.Indented);
                }
                catch(Exception e)
                {
                    output = "Error: " + e.Message;
                }

                // print
                Console.WriteLine(output);

            } // loop!
        }

        private static void CompileToDll(string[] args)
        {
            var file = File.ReadAllText(args[1]);

            Expression programExpression = Evaluate(file);
            var param = Expression.Parameter(typeof(string));
            var mainMethod = Expression.Lambda<Action<string[]>>(programExpression, param);
            var compiled = mainMethod.Compile();
            //todo: create a program with the compiled method as the 'Main' method, output DLL.
        }

        private static Expression Evaluate(string input)
        {
            BuiltInFunctions.AddBuiltinFunctions(GlobalScope);
            Expression programExpression;
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
