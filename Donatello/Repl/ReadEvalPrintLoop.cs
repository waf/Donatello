using Donatello.Compilation;
using Donatello.Parser;
using Donatello.StandardLibrary;
using Donatello.Util;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Repl
{
    public class ReadEvalPrintLoop
    {
        public async Task RunAsync()
        {
            var initialState = Task.Run(InitialEvaluation).ConfigureAwait(false);
            DrawBanner();
            ScriptState<object> state = null;
            while (true)
            {
                //read
                Console.Write("> ");
                string text = Console.ReadLine().Trim();
                if (text == string.Empty) { continue; }
                if (text == "exit") { break; }

                try
                {
                    // eval
                    // convert to roslyn tree
                    var program = AntlrParser.ParseAsRepl(text)
                        .NormalizeWhitespace()
                        .ToFullString();
                    // pass roslyn tree to scripting api
                    state = await (state ?? await initialState)
                        .ContinueWithAsync(program) // roslyn scripting api - continue program with existing instance state
                        .ConfigureAwait(false); // tpl api - await configuration

                    // print!
                    ReplPrinter.Print(state.ReturnValue);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Error: " + e.Message);
                }
            } // loop!
        }

        private static void DrawBanner()
        {
            Console.WriteLine("Welcome to the Donatello REPL. Try typing (+ 2 2) or type exit to leave.");
            Console.WriteLine();
        }

        /// <summary>
        /// Initial REPL setup and warmup
        /// </summary>
        private async static Task<ScriptState<object>> InitialEvaluation()
        {
            // import required libraries
            var usings = Compiler.DefaultImports.Keys
                .Select(ns => $"using {ns};")
                .StringJoin(" ");
            var references = ScriptOptions.Default.WithReferences(Compiler.GetDefaultReferences());
            var state = await CSharpScript.RunAsync(usings, references).ConfigureAwait(false);

            // do a first run to "warm up" the repl so the user doesn't experience a delay for the first evaluation.
            var warmup = AntlrParser.ParseAsRepl(@"""Hello World""")
                .NormalizeWhitespace()
                .ToFullString();
            return await state.ContinueWithAsync(warmup).ConfigureAwait(false);
        }
    }
}
