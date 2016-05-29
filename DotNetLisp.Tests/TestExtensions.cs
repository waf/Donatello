using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetLisp.Tests
{
    static class TestExtensions
    {
        public static void AssertOutput<T>(string test, T expected)
        {
            var result = Program.Run<T>(test);
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

