using System;
using Xunit;
using static Donatello.Tests.TestExtensions;

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
                    (let [list (new ListContext)
                          not  (new ListContext)]
                        (.AddChild list (quote ""if""))
                        (.AddChild list not)
                        (.AddChild not (quote ""not""))
                        (.AddChild not condition)
                        (.AddChild list falseBranch)
                        (.AddChild list trueBranch)))

               (unless false 1 0)
            ";
            AssertOutput(code, 1);
        }
    }
}
