using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void FunctionsCanHaveMultipleLines()
        {
            const string code =
                @"
                    (fun foo [x:int y:int] :int
                      (Console.WriteLine x)
                      (+ x y))

                    (foo 4 6)
                ";
            AssertOutput(code, 10);
        }

        [Fact]
        public void FunctionsCanBeInvoked()
        {
            const string code =
                @"
                    (fun foo [x:int y:int] :int
                      (+ x y))

                    (foo 4 6)
                ";
            AssertOutput(code, 10);
        }

        [Fact]
        public void FunctionsCanBeOverloaded()
        {
            const string code =
                @"
                    (fun foo [x:int y:int] :int
                      (+ x y))

                    (fun foo [x:int y:int z:int] :int
                      (+ x y z))

                    (+
                        (foo 4 6)
                        (foo 4 6 2))
                ";
            AssertOutput(code, 22);
        }

        [Fact]
        public void FunctionsCanAccessOuterScope()
        {
            const string code =
                @"
                    (def a:int 8)
                    (def b:int 12)

                    (fun foo [x:int] :int
                      (+ x a b))

                    (foo 5)
                ";
            AssertOutput(code, 25);
        }
    }
}
