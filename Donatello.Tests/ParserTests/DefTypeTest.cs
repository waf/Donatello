using Donatello.Ast;
using Donatello.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;
using static Donatello.Parser.Generated.DonatelloParser;
using static Donatello.Tests.TestHelpers;

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

        [TestMethod]
        public void DefType_Compiled_ProducesType()
        {
            var assembly = CompileProgram("(deftype Cat -color String -name String)");
            var cat = assembly.GetType("Cat");

            Assert.IsNotNull(cat);
            var properties = cat.GetProperties();
            Assert.AreEqual(2, properties.Length);
            AssertProperty("String", "color", properties[0]);
            AssertProperty("String", "name", properties[1]);

            void AssertProperty(string type, string name, PropertyInfo property)
            {
                Assert.AreEqual(type, property.PropertyType.Name);
                Assert.AreEqual(name, property.Name);
            }
        }

        [TestMethod]
        public void DefType_TypeProduced_CanBeInstantiated()
        {
            var assembly = CompileProgram("(deftype Cat -color String -name String)");
            var catType = assembly.GetType("Cat");

            var cat = Activator.CreateInstance(catType, "white", "Fluffy");
        }
    }
}
