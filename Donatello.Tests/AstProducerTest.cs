using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Donatello.Parser;
using Donatello.Ast;
using System.Linq;

namespace Donatello.Tests
{
    [TestClass]
    public class AstProducerTest
    {
        [TestMethod]
        public void Def()
        {
            var ast = AstProducer.Parse(@"(def a 5)");
            var line = (ast as FileUntypedExpression).Statements.Single();

            var def = AssertIsType<DefUntypedExpression>(line);
            var body = AssertIsType<LongExpression>(def.Body);
            Assert.AreEqual("a", def.Symbol.Name);
            Assert.AreEqual(5, body.Value);
        }

        [TestMethod]
        public void Defn()
        {
            var ast = AstProducer.Parse(@"(defn add [a b] (+ a b)");
            var line = (ast as FileUntypedExpression).Statements.Single();

            var fn = AssertIsType<FunctionUntypedExpression>(line);
            Assert.AreEqual("add", fn.Name);
            Assert.AreEqual(2, fn.Arguments.Count);
            Assert.AreEqual("a", fn.Arguments[0].Name);
            Assert.AreEqual("b", fn.Arguments[1].Name);

            // one list with three elements
            Assert.AreEqual(1, fn.Body.Count);
            var body = AssertIsType<ListUntypedExpression>(fn.Body[0]);
            Assert.AreEqual(3, body.Elements.Count);
            var elementTests = body.Elements
                .Zip("+ab", (parsedElement, expectedName) => (parsedElement, expectedName));
            foreach (var test in elementTests)
            {
                var symbol = AssertIsType<SymbolUntypedExpression>(test.parsedElement);
                Assert.AreEqual(test.expectedName.ToString(), symbol.Name);
            }
        }

        private T AssertIsType<T>(object value)
        {
            Assert.IsInstanceOfType(value, typeof(T));
            return (T)value;
        }
    }
}
