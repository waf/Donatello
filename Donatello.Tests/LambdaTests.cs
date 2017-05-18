using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.TestExtensions;

namespace Donatello.Tests
{
    public class LambdaTests
    {
        [Fact]
        public void SimpleInvocation()
        {
            const string code = @"
                (use System.Linq)
                (Enumerable.Select [1 2 3 4] \(+ 1 \0))
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
        public void NestedInvocation()
        {
            const string code = @"
                (use System.Linq)
                (Enumerable.Select [[1] [2]]
                    \(Enumerable.Select \0 \(+ 1 \1)))
            ";
            AssertOutput<IEnumerable<IEnumerable<int>>>(code, result =>
            {
                var output = result.Select(r => r.ToList()).ToList();
                Assert.Equal(2, output[0][0]);
                Assert.Equal(3, output[1][0]);
            });
        }
    }
}
