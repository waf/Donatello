using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Donatello.Tests.TestExtensions;
using Xunit;
using static System.Linq.Enumerable;

namespace Donatello.Tests
{
    public class UseTests
    {

        [Fact]
        public void StaticUsings()
        {
            const string code =
                @"
                    (use System.Int32.*)

                    MaxValue
                ";
            AssertOutput(code, int.MaxValue);
        }

        [Fact]
        public void QualifedUsingsForClass()
        {
            const string code =
                @"
                    (use System)

                    Int32.MaxValue
                ";
            AssertOutput(code, int.MaxValue);
        }

        [Fact]
        public void QualifedUsingsForNamespace()
        {
            const string code =
                @"
                    (use System.IO)

                    (Path.Combine ""Hello"" ""World"")
                ";

            AssertOutput(code, System.IO.Path.Combine("Hello", "World"));
        }
    }
}
