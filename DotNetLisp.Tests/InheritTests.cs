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
                (inherit IComparable<Runner>)
                (defn GetX [] :int 5)
                (defn CompareTo [obj:Runner] :int
                    (.CompareTo (.GetX obj) (.GetX this)))

                (def example:IComparable<Runner> (new Runner))
            ";

        }
    }
}
