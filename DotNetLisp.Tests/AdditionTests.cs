using DotNetLisp.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DotNetLisp.Tests.TestExtensions;
using Xunit;

namespace DotNetLisp.Tests
{
    public class AdditionTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void AddTwoNumbers(int x, int y)
        {
            AssertOutput($"(+ {x} {y})", x + y);
            return;
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { int.MinValue, int.MaxValue, 10, 0 })]
        public void AddRangeOfNumbers(int[] xs)
        {
            var arguments = " ".Join(xs);
            AssertOutput($"(+ {arguments})", xs.Sum());
        }
    }
}
