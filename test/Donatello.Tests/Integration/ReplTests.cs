using Donatello.Repl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.Integration.TestExtensions;

namespace Donatello.Tests.Integration
{
    public class ReplTests
    {

        /*
        [Fact]
        public void ReplEvaluation_ParseError_CanContinue()
        {
            var expectedOutputAndInput = new string[][] {
                new[] { "> ",      @"a"},
                new[] { "Error: CS0103: The name 'a' does not exist in the current context\r\n", null },
                new[] { "> ",      @"(def a:int 5)"},
                new[] { "> ",      @"a"},
                new[] { "5 :Int32\r\n",   null },
                new[] { "> ",      @"exit"}
            };
            AssertReplSession(expectedOutputAndInput);
        }

        private static void AssertReplSession(string[][] expectedOutputAndInput)
        {
            var expectedOutput = string.Join("", expectedOutputAndInput.Select(io => io[0]));
            var inputValues = expectedOutputAndInput.Select(io => io[1]).Where(s => s != null);

            // rebind console input and output so we can send/capture it programmatically.
            var output = new StringBuilder();
            Console.SetIn(new StringReader(string.Join(Environment.NewLine, inputValues)));
            Console.SetOut(new StringWriter(output));

            var repl = new ReadEvalPrintLoop();
            repl.Run();

            Assert.Equal(expectedOutput, output.ToString());
        }
        */
    }
}
