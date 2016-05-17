using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void FunctionsCanBeInvoked()
        {
            const string code =
                @"
                    (fun foo [x:int y:int] :int
                      (+ x y))

                    (foo 4 6)
                ";
            Test(code, 10);
        }

    }
}
