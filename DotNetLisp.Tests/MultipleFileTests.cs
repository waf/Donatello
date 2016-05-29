using DotNetLisp.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetLisp.Tests
{
    public class MultipleFileTests
    {
        [Fact]
        public void MultipleFiles()
        {
            Program.CompileContent(new Dictionary<Tuple<string, string>, string>
            {
                { Tuple.Create("UnitTest", "Aaa"), @"(Console.WriteLine ""Hello World"")" },
                { Tuple.Create("UnitTest", ""), @"(Console.WriteLine ""Hello World"")" }
            }, "Out", OutputType.DynamicallyLinkedLibrary);
        }
    }
}
