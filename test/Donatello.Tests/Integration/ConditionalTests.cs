using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static Donatello.Tests.Integration.TestExtensions;

namespace Donatello.Tests.Integration
{
    public class ConditionalTests
    {
        [Fact]
        public void IfStatement_IsTrue_SelectsTrueBranch()
        {
            AssertOutput("(if true 5 10)", 5);
        }

        [Fact]
        public void IfStatement_IsFalse_SelectsFalseBranch()
        {
            AssertOutput("(if false 5 10)", 10);
        }
    }
}
