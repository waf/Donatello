using CommandLine;
using CommandLine.Text;
using DotNetLisp.Compilation;
using DotNetLisp.Parser;
using DotNetLisp.Repl;
using DotNetLisp.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotNetLisp
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
                CompileFile(options.Inputs.ToArray(), options.Output);
                return;
            }

            var repl = new ReadEvalPrintLoop(Console.ReadLine, Console.Write);
            repl.Run();
        }

        private static void CompileFile(string[] inputFile, string outputDll)
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
            var result = CompileContent(fileContent, Path.GetFileNameWithoutExtension(outputDll));
            result.Match(
                Ok: bytes => File.WriteAllBytes(outputDll, bytes),
                Error: errors => Console.WriteLine(string.Join(Environment.NewLine, errors)));
        }

        public static Result<byte[], string[]> CompileContent(IDictionary<Tuple<string, string>, string> files, string dllName)
        {
            var result = files
                .Select(file => AntlrParser.Parse(file.Value, file.Key.Item1, file.Key.Item2, "Main"))
                .ToArray();

            var assembly = Compiler.Compile(dllName, result);

            return assembly;
        }

        public static Result<T, string[]> Run<T>(string program)
        {
            const string namespaceName = "DotNetLispRun";
            const string className = "Runner";
            const string methodName = "Run";

            var result = AntlrParser.Parse(program, namespaceName, className, methodName);
            var compiled = Compiler.Compile(namespaceName, result);

            return compiled.Select(bytes =>
                AssemblyRunner.Run<T>(bytes, namespaceName, className, methodName)
            );
        }

        private class CommandLineOptions
        {
            [OptionList('i', "input", HelpText = "A list of input files")]
            public IList<string> Inputs { get; set; } = new List<string>();

            [Option('o', "output", DefaultValue = "Out.dll", HelpText = "The output DLL name")]
            public string Output { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }
    }
}
