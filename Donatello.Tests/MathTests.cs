using Donatello.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Donatello.Tests.TestExtensions;
using Xunit;

namespace Donatello.Tests
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
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(int.MaxValue, int.MaxValue / 2)]
        public void SubtractTwoNumbers(int x, int y)
        {
            AssertOutput($"(- {x} {y})", x - y);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(-1, 1)]
        [InlineData(4, 6)]
        public void MultiplyTwoNumbers(int x, int y)
        {
            AssertOutput($"(* {x} {y})", x * y);
        }

        [Theory]
        [InlineData(-1, 1)]
        [InlineData(6, 4)]
        public void DivideTwoNumbers(int x, int y)
        {
            AssertOutput($"(/ {x} {y})", x / y);
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { int.MinValue, int.MaxValue, 10, 0 })]
        public void AddRangeOfNumbers(int[] xs)
        {
            var arguments = " ".Join(xs);
            AssertOutput($"(+ {arguments})", xs.Sum());
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { int.MaxValue, int.MaxValue / 2, 10, 0 })]
        public void SubtractRangeOfNumbers(int[] xs)
        {
            var arguments = " ".Join(xs);
            AssertOutput($"(- {arguments})", xs.Aggregate((a, b) => a - b));
        }

        [Theory]
        [InlineData(new[] { 1, 2, 3 })]
        [InlineData(new[] { 5, 2, 0 })]
        [InlineData(new[] { int.MaxValue / 6, 2, 3 })]
        public void MultiplyRangeOfNumbers(int[] xs)
        {
            var arguments = " ".Join(xs);
            AssertOutput($"(* {arguments})", xs.Aggregate((a, b) => a * b));
        }

        [Theory]
        [InlineData(new[] { 0, 2, 3 })]
        [InlineData(new[] { 10, 5, 2 })]
        [InlineData(new[] { int.MaxValue, 2, 3 })]
        public void DivideRangeOfNumbers(int[] xs)
        {
            var arguments = " ".Join(xs);
            AssertOutput($"(/ {arguments})", xs.Aggregate((a, b) => a / b));
        }
    }
}
