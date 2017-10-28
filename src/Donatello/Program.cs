using CommandLine;
using Donatello.Build;
using Donatello.Repl;
using Donatello.Services.Compilation;
using Donatello.Services.Parser;
using Donatello.Services.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Donatello
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithNotParsed(errors => Environment.Exit(1))
                .WithParsed(options =>
                {
                    if(options.Inputs.Any())
                    {
                        CompileInputs(options.Inputs, options.References, options.Output);
                        return;
                    }

                    new ReadEvalPrintLoop()
                        .RunAsync()
                        .Wait();
                });
        }

        private static void CompileInputs(IList<string> inputs, IList<string> references, string output)
        {
            string[] referenceFilePaths = references
                .Select(reference => new DirectoryInfo(reference).FullName)
                .ToArray();

            // todo: do something better?
            foreach (var reference in referenceFilePaths)
            {
                AssemblyLoadContext.Default.LoadFromAssemblyPath(reference);
            }

            FileCompiler.CompileFiles(inputs.ToArray(), referenceFilePaths, output);
        }

        /// <summary>
        /// Run the provided donatello program and return the result.
        /// </summary>
        /// <typeparam name="T">The expected return type</typeparam>
        /// <param name="program">The donatello source code</param>
        /// <returns>the return value of the program</returns>
        public static T Run<T>(string program, string assemblyName = null, params string[] references)
        {
            const string namespaceName = "DonatelloRun";
            const string className = "Runner";
            const string methodName = "Run";

            CompilationUnitSyntax syntaxTree = AntlrParser.ParseAsClass(program, namespaceName, className, methodName);
            Stream result = Compiler.Compile(assemblyName ?? namespaceName, references, OutputType.DynamicallyLinkedLibrary, syntaxTree);

            return AssemblyRunner.Run<T>(result, namespaceName, className, methodName);
        }

        /// <summary>
        /// Command line flag schema
        /// </summary>
        private class CommandLineOptions
        {
            [Option('i', "input", HelpText = "A list of input files")]
            public IList<string> Inputs { get; set; } = new List<string>();

            [Option('o', "output", Default = "Out.exe", HelpText = "The output DLL name")]
            public string Output { get; set; }

            [Option('r', "references", HelpText = "The DLLs to reference")]
            public IList<string> References { get; set; } = new List<string>();
        }
    }
}
