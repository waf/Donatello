using Donatello.Services.Parser;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Text.RegularExpressions;
using Xunit;

namespace Donatello.Tests.Unit
{
    public class TransformTest
    {
        static readonly Regex Whitespace = new Regex(@"\s+");

        [Theory]
        [InlineData("Let")]
        [InlineData("Macro")]
        [InlineData("PlusSymbol")]
        public void Donatello_Transforms_ToCSharp(string test)
        {
            string file = Path.Combine("Unit", "Transforms", test);

            string donatello = File.ReadAllText(file + ".dnl");
            string expectedCSharp = File.ReadAllText(file + ".cs");

            string actualCSharp = AntlrParser.ParseAsClass(donatello, "UnitTest", test + "Test")
                .NormalizeWhitespace()
                .ToFullString();

            string normalizedExpectedCSharp = Whitespace.Replace(expectedCSharp, "");
            string normalizedActualCSharp = Whitespace.Replace(actualCSharp, "");

            Assert.Equal(normalizedExpectedCSharp, normalizedActualCSharp);
            
        }
    }
}
