using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.Integration.TestExtensions;

namespace Donatello.Tests.Integration
{
    public class InstanceTests
    {
        [Fact]
        public void CanCreateInstance()
        {
            const string code = @"
                (instance)
                (defn X [] :int 5)

                (.X (new Runner))
            ";
            AssertOutput(code, 5);
        }

        [Fact]
        public void InterfaceImplementation()
        {
            const string code = @"
                (instance IComparable<Runner>)
                (defn GetX [] :int 5)
                (defn CompareTo [obj:Runner] :int
                    (.CompareTo (.GetX obj) (.GetX this)))

                (let [a (new Runner)
                      b (new Runner)]
                    (.CompareTo a b))
            ";
            AssertOutput(code, 0);
        }
    }
}
