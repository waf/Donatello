using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.TestExtensions;

namespace Donatello.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void FunctionsCanHaveMultipleLines()
        {
            const string code =
                @"
                    (defn foo [x:int y:int] :int
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
                    (defn foo [x:int y:int] :int
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
                    (defn foo [x:int y:int] :int
                      (+ x y))

                    (defn foo [x:int y:int z:int] :int
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

                    (defn foo [x:int] :int
                      (+ x a b))

                    (foo 5)
                ";
            AssertOutput(code, 25);
        }

        [Fact]
        public void AnonymousFunction()
        {
            const string code =
                @"
                    (use System.Linq)
                    (Enumerable.Select [1 2 3 4] (fn [i x] (+ 1 i)))
                ";
            AssertOutput<IEnumerable<int>>(code, result =>
            {
                var computed = result.ToList();
                Assert.Equal(2, computed[0]);
                Assert.Equal(3, computed[1]);
                Assert.Equal(4, computed[2]);
                Assert.Equal(5, computed[3]);
            });
        }

        [Fact]
        public void AnonymousFunctionWithMultipleLines()
        {
            const string code =
                @"
                    (use System.Linq)
                    (Enumerable.Select [1 2 3 4]
                        (fn [i x]
                            (Console.WriteLine x)
                            (+ 1 i)))
                ";
            AssertOutput<IEnumerable<int>>(code, result =>
            {
                var computed = result.ToList();
                Assert.Equal(2, computed[0]);
                Assert.Equal(3, computed[1]);
                Assert.Equal(4, computed[2]);
                Assert.Equal(5, computed[3]);
            });
        }

        [Fact]
        public void InstanceMethods()
        {
            const string code =
                @"
                    (.Replace ""Hello World"" ""World"" ""Universe"")
                ";
            AssertOutput(code, "Hello Universe");

        }
    }
}
