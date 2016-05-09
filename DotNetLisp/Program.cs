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
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DotNetLisp
{
    static class Program
    {
        internal readonly static Scope GlobalScope = new Scope();

        static void Main(string[] args)
        {
            if (args.Length > 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("  DotNetLisp           open a repl");
                Console.WriteLine("  DotNetLisp <file>    compile a file");
            }

            if(args.Length > 0)
            {
                CompileFile(args);
                return;
            }

            ReadEvalPrintLoop();
        }

        private static void CompileFile(string[] files)
        {
            var file = files[0];
            var namespaceName = Directory.GetParent(file).Name;
            var className = Path.GetFileNameWithoutExtension(file);
            var result = AntlrParser.Parse(File.ReadAllText(file), namespaceName, className);
            var assembly = Compiler.Compile(result);

            assembly.Match(
                Ok: bytes => File.WriteAllBytes(file.Replace("dnl", "dll"), bytes),
                Error: errors => Console.WriteLine(string.Join(Environment.NewLine, errors)));
        }

        private static void ReadEvalPrintLoop()
        {
            const string namespaceName = "Repl";
            const string className = "Program";
            // there's a slight delay when we load up roslyn and run a program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience a delay for the first evaluation.
            Task.Run(() => Compiler.CompileForRepl(AntlrParser.Parse("(+ 1 1)", "DotNetLisp", "Warmup")));

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
                    var ast = AntlrParser.Parse(input, namespaceName, className);
                    var assembly = Compiler.CompileForRepl(ast);
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
    }
}
