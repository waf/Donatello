using CommandLine;
using CommandLine.Text;
using Donatello.Repl;
using Donatello.Services.Compilation;
using Donatello.Services.Parser;
using Donatello.Services.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Donatello
{
    public static class Program
    {
        static void Main(string[] args)
        {
            var options = new CommandLineOptions();

            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                Console.WriteLine(options.GetUsage());
                return;
            }

            if(options.Inputs.Any())
            {
                CompileFile(options.Inputs.ToArray(), options.References.ToArray(), options.Output);
                return;
            }

            var repl = new ReadEvalPrintLoop();
            repl.RunAsync().Wait();
        }

        private static void CompileFile(string[] inputFile, string[] references, string outputFilename)
        {
            var fileContent = (from file in inputFile
                               select new
                               {
                                   NamespaceName = Directory.GetParent(file).Name,
                                   ClassName = Path.GetFileNameWithoutExtension(file),
                                   Content = File.ReadAllText(file)
                               })
                              .ToDictionary(
                                    unit => Tuple.Create(unit.NamespaceName, unit.ClassName),
                                    unit => unit.Content);
            string assemblyName = Path.GetFileNameWithoutExtension(outputFilename);
            string extension = Path.GetExtension(outputFilename);
            var outputKind = extension == ".dll" ? OutputType.DynamicallyLinkedLibrary :
                             extension == ".exe" ? OutputType.ConsoleApplication :
                             throw new Exception("unknown extension");
            var bytes = CompileContent(fileContent, references, assemblyName, outputKind);
            File.WriteAllBytes(outputFilename, bytes);
        }

        public static byte[] CompileContent(
            IDictionary<Tuple<string, string>, string> files,
            IList<string> references,
            string assemblyName,
            OutputType outputKind)
        {
            var compilationUnit = files
                .Select(file => AntlrParser.ParseAsClass(file.Value, file.Key.Item1, file.Key.Item2) as CompilationUnitSyntax)
                .ToArray();

            var assembly = Compiler.Compile(assemblyName, references, outputKind, compilationUnit);

            return assembly;
        }

        public static T Run<T>(string program)
        {
            const string namespaceName = "DonatelloRun";
            const string className = "Runner";
            const string methodName = "Run";

            var result = AntlrParser.ParseAsClass(program, namespaceName, className, methodName) as CompilationUnitSyntax;
            var bytes = Compiler.Compile(namespaceName, new string[0], OutputType.DynamicallyLinkedLibrary, result);

            return AssemblyRunner.Run<T>(bytes, namespaceName, className, methodName);
        }

        private class CommandLineOptions
        {
            [OptionList('i', "input", HelpText = "A list of input files")]
            public IList<string> Inputs { get; set; } = new List<string>();

            [Option('o', "output", DefaultValue = "Out.exe", HelpText = "The output DLL name")]
            public string Output { get; set; }

            [OptionList('r', "references", HelpText = "The DLLs to reference")]
            public IList<string> References { get; set; } = new List<string>();

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}
