using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class InheritTests
    {
        [Fact]
        public void Inherit()
        {
            const string code = @"
                (inherit IComparable)
                (defn CompareTo [obj:Object] :int
                    0)
            ";



        }
    }
}
