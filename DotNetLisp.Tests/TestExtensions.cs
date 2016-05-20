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
            var sum = Program.Run<T>(test);
            sum.Match(
                Ok: result => Assert.Equal(expected, result),
                Error: errors => {
                    throw new Exception(string.Join(Environment.NewLine, errors));
                });

        }
        public static void AssertOutput<T>(string test, Action<T> assertions)
        {
            var sum = Program.Run<T>(test);
            sum.Match(
                Ok: result => assertions(result),
                Error: errors => {
                    throw new Exception(string.Join(Environment.NewLine, errors));
                });

        }

        public static string Join<T>(this string separator, T[] values)
        {
            return string.Join(separator, values);
        }
    }
}

