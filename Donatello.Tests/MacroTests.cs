using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotNetLisp.Tests
{
    class MacroTests
    {
        [Fact]
        public void MacroTest()
        {
            const string code = @"
               (defmacro unless [condition:IParseTree
                                 falseBranch:IParseTree
                                 trueBranch:IParseTree]
                         :IParseTree
                    (ImmutableList.Create (quote if)
                                            
            ";
            AssertOutput(code, 10);
        }
    }
}
