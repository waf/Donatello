using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class LetTests
    {
        [Fact]
        public void SimpleLet()
        {
            const string code = @"
                (let [a 6
                      b 4]
                    (+ a b))
            ";
            AssertOutput(code, 10);
        }

        [Fact]
        public void MultiLineLet()
        {
            const string code = @"
                (let [a 6
                      b 4]
                    (Console.WriteLine a)
                    (+ a b))
            ";
            AssertOutput(code, 10);
        }
        public static T CreateLet<T>(Func<T> letFunc)
        {
            return letFunc();
        }

        [Fact]
        public void NestedLet()
        {
            const string code = @"
                (let [a (let [x 2] (+ x 1))
                      b (let [y 5] (+ a y))]
                    (+ a b))
            ";
            AssertOutput(code, 11);
        }

    }
}
