using DotNetLisp.Repl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class ReplTests
    {

        [Fact]
        public void ReplEvaluation()
        {
            var expectedOutputAndInput = new string[][] {
                new[] { "> ",      @"(def a:int 5)"},
                new[] { "> ",      @"(def b:int 7)"},
                new[] { "> ",      @"(fun mystery [x:int] :int 
                                       (+ x a b))"},
                new[] { "> ",      @"(mystery 8)"},
                new[] { "20\r\n",  null },

                new[] { "> ",      @"(def b:int 8)"},
                new[] { "> ",      @"(def c:int 10)"},
                new[] { "> ",      @"(fun mystery [x:int] :int 
                                       (+ x a b c))"},
                new[] { "> ",      @"(mystery 2)"},
                new[] { "25\r\n",  null },
                new[] { "> ",      @"exit"}
            };
            AssertReplSession(expectedOutputAndInput);
        }

        [Fact]
        public void ReplEvaluation_ParseError_CanContinue()
        {
            var expectedOutputAndInput = new string[][] {
                new[] { "> ",      @"a"},
                new[] { "CS0103: The name 'a' does not exist in the current context\r\n", null },
                new[] { "> ",      @"(def a:int 5)"},
                new[] { "> ",      @"a"},
                new[] { "5\r\n",   null },
                new[] { "> ",      @"exit"}
            };
            AssertReplSession(expectedOutputAndInput);
        }

        private static void AssertReplSession(string[][] expectedOutputAndInput)
        {
            var expectedOutput = expectedOutputAndInput.Select(io => io[0]);
            var inputValues = expectedOutputAndInput.Select(io => io[1]).Where(s => s != null);

            var enumerator = inputValues.GetEnumerator();
            var output = new List<string>();

            var repl = new ReadEvalPrintLoop(
                () => { enumerator.MoveNext(); return (string)enumerator.Current; },
                (str) => output.Add(str));

            repl.Run();

            Assert.Equal(expectedOutput, output);
        }
    }
}
