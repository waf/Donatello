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
using DotNetLisp.Repl;

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

            ReadEvalPrintLoop.Run();
        }

        private static void CompileFile(string[] files)
        {
            var file = files[0];
            var namespaceName = Directory.GetParent(file).Name;
            var className = Path.GetFileNameWithoutExtension(file);
            var result = AntlrParser.Parse(File.ReadAllText(file), namespaceName, className, "Main");
            var assembly = Compiler.Compile(result);

            assembly.Match(
                Ok: bytes => File.WriteAllBytes(file.Replace("dnl", "dll"), bytes),
                Error: errors => Console.WriteLine(string.Join(Environment.NewLine, errors)));
        }

    }
}
