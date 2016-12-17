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
            // there's a slight delay (2-3 seconds) when we load up roslyn and run a program for the first time. Do an
            // initial run to warm things up, so the user doesn't experience a delay for the first evaluation.
            var warmup = Task.Run(() => CSharpScript.RunAsync(@"""Hello World"""));
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
                    var program = AntlrParser.ParseAsRepl(text)
                        .NormalizeWhitespace()
                        .ToFullString();
                    state = await (state ?? await warmup).ContinueWithAsync(program);
                    // print!
                    ReplUtil.Print(state.ReturnValue);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Error: " + e.Message);
                }
            } // loop!
        }
    }
}
