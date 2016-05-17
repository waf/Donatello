﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static DotNetLisp.Tests.TestExtensions;

namespace DotNetLisp.Tests
{
    public class ConditionalTests
    {
        [Fact]
        public void IfStatement_IsTrue_SelectsTrueBranch()
        {
            Test("(if true 5 10)", 5);
        }

        [Fact]
        public void IfStatement_IsFalse_SelectsFalseBranch()
        {
            Test("(if false 5 10)", 10);
        }
    }
}
