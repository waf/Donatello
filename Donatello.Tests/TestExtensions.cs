using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Donatello.Tests
{
    static class TestExtensions
    {
        public static void AssertOutput<T>(string test, T expected, params string[] references)
        {
            var result = Program.Run<T>(test, references);
            Assert.Equal(expected, result);
        }
        public static void AssertOutput<T>(string test, Action<T> assertions)
        {
            var result = Program.Run<T>(test);
            assertions(result);
        }

        public static string Join<T>(this string separator, T[] values)
        {
            return string.Join(separator, values);
        }
    }
}

