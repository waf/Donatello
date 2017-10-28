using Donatello.Build;
using Donatello.Services.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Donatello.Tests.Integration
{
    public class MultipleFileTests
    {
        [Fact]
        public void MultipleFiles()
        {
            FileCompiler.CompileSource(new[]
            {
                ("UnitTest", "Aaa", @"(Console.WriteLine ""Hello World"")"),
                ("UnitTest", "",    @"(Console.WriteLine ""Hello World"")")
            }, new string[0], "Out", OutputType.DynamicallyLinkedLibrary);
        }
    }
}
