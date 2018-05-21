using Antlr4.Runtime;
using Donatello.Ast;
using Donatello.Parser;
using Donatello.Parser.Generated;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Donatello.Parser.Generated.DonatelloParser;

namespace Donatello.Tests.ParserTests
{
    [TestClass]
    public class DefTypeTest
    {
        [TestMethod]
        public void DefType_Parse()
        {
            string program = "(deftype Cat -color string -name string)";
            var file = Parse<DefTypeContext>(program);

            Assert.AreEqual("Cat", file.identifier().GetText());

            var declarations = file.propertyDeclaration();

            Assert.AreEqual("-color", declarations[0].property().GetText());
            Assert.AreEqual("string", declarations[0].type().GetText());

            Assert.AreEqual("-name",  declarations[1].property().GetText());
            Assert.AreEqual("string", declarations[1].type().GetText());

            return;
        }

        [TestMethod]
        public void DefType_ProduceAST()
        {
            var parsedDefType = Parse<DefTypeContext>("(deftype Cat -color string -name string)");
            var expression = new AstProducer().VisitDefType(parsedDefType);

            Assert.IsInstanceOfType(expression, typeof(DefTypeExpression));
            var defType = (DefTypeExpression)expression;

            Assert.AreEqual("Cat", defType.Identifier);
            Assert.AreEqual(2, defType.Properties.Count);
            Assert.AreEqual("color", defType.Properties[0].Identifier);
            Assert.AreEqual("string", defType.Properties[0].Type);
            Assert.AreEqual("name", defType.Properties[1].Identifier);
            Assert.AreEqual("string", defType.Properties[1].Type);
        }

        private static T Parse<T>(string program)
            where T : ParserRuleContext
        {
            AntlrInputStream inputStream = new AntlrInputStream(program);
            DonatelloLexer lexer = new DonatelloLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            DonatelloParser parser = new DonatelloParser(tokenStream);
            var parsed = parser.file()
                .GetChild(0)  // first line of file
                .GetChild(0); // child of Form

            Assert.IsInstanceOfType(parsed, typeof(T));

            return (T)parsed;
        }
    }
}
