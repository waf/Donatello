using Donatello.Ast;
using Donatello.Parser;
using Donatello.Services;
using Donatello.TypeInference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Donatello.Tests
{
	[TestClass]
    public class TypeInferenceTest
    {
        [TestMethod]
        public void Def_WithChain_InfersTypes()
        {
            var defines = InferTypes("(def a 5) (def b a)")
                .Find<DefExpression>()
                .ToList();

            Assert.AreEqual(2, defines.Count);

            // assert the type of the symbol
            Assert.AreEqual(new ConcreteType(typeof(long)), defines[0].Symbol.Type);
            Assert.AreEqual(new ConcreteType(typeof(long)), defines[1].Symbol.Type);

            // assert the type of the entire expression
            Assert.AreEqual(new ConcreteType(typeof(long)), defines[0].Type);
            Assert.AreEqual(new ConcreteType(typeof(long)), defines[1].Type);

            return;
        }

        [DataTestMethod]
        [DataRow("(add 5 3)")]
        [DataRow("(add 5 3) (add 3 9)")]
        public void Function_WithLiteralParameters_InfersFunctionType(string program)
        {
            var (function, param1, param2) = InferTypes(program)
                .Find<SymbolExpression, LongExpression, LongExpression>()
                .First();

            Assert.AreEqual("add", function.Name);
            Assert.IsInstanceOfType(function.Type, typeof(FunctionType));

            // function type should be long -> long -> 'c
            var functionType = (FunctionType)function.Type;
            Assert.AreEqual(2, functionType.ArgumentTypes.Length);
            Assert.AreEqual(param1.Type, functionType.ArgumentTypes[0]);
            Assert.AreEqual(param2.Type, functionType.ArgumentTypes[1]);
            Assert.AreEqual(new ConcreteType(typeof(long)), param2.Type);
            Assert.IsInstanceOfType(functionType.ReturnType, typeof(TypeVariable));
        }

        [TestMethod]
        public void Function_WithSymbolParameters_InfersType()
        {
            var (function, param1, param2) = InferTypes("(def a 5) (def b 6) (add a b)")
                .Find<SymbolExpression, SymbolExpression, SymbolExpression>()
                .Single();

            var functionType = (FunctionType)function.Type;
            Assert.AreEqual(2, functionType.ArgumentTypes.Length);
            Assert.AreEqual(param1.Type, functionType.ArgumentTypes[0]);
            Assert.AreEqual(param2.Type, functionType.ArgumentTypes[1]);
            Assert.AreEqual(new ConcreteType(typeof(long)), param2.Type);
            Assert.IsInstanceOfType(functionType.ReturnType, typeof(TypeVariable));
        }


        private static Ast.ITypedExpression InferTypes(string source)
        {
            var ast = AstProducer.Parse(source);
            var typed = HindleyMilner.Infer(ast);
            return typed;
        }
    }
}
