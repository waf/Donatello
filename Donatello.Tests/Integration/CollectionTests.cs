using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.Integration.TestExtensions;

namespace Donatello.Tests.Integration
{
    public class CollectionTests
    {
        [Fact]
        public void LiteralArray()
        {
            AssertOutput<ImmutableArray<int>>("[1 2 3 4]",
                (array) =>
                {
                    Assert.Equal(1, array[0]);
                    Assert.Equal(2, array[1]);
                    Assert.Equal(3, array[2]);
                    Assert.Equal(4, array[3]);
                });
        }

        [Fact]
        public void LiteralDictionary()
        {
            AssertOutput<ImmutableDictionary<string, int>>(
                @"{""a"" 1
                   ""b"" 2
                   ""c"" 3
                   ""d"" 4}",
                (dict) =>
                {
                    Assert.Equal(1, dict["a"]);
                    Assert.Equal(2, dict["b"]);
                    Assert.Equal(3, dict["c"]);
                    Assert.Equal(4, dict["d"]);
                });
        }

        [Fact]
        public void LiteralSet()
        {
            AssertOutput<ImmutableHashSet<int>>(
                @"| 1 2 3 1 3 |",
                (set) =>
                {
                    Assert.Equal(3, set.Count);
                    Assert.Contains(1, set);
                    Assert.Contains(2, set);
                    Assert.Contains(3, set);
                });
        }
    }
}
