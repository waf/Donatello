using Donatello.Parser;
using Donatello.TypeInference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Tests
{
    [TestClass]
    public class TypeInferenceTest
    {
        [TestMethod]
        public void InferType()
        {
            string source = "(def a 5) (def b a) (def c \"hello world\")";
            var ast = AstProducer.Parse(source);
            return;
        }

        [TestMethod]
        public void InferType2()
        {
            string source = "(def a 5) (def b 6) (add a b)";
            var ast = AstProducer.Parse(source);
            return;
        }

        [TestMethod]
        public void ConstraintTest()
        {
            string source = "(add 5 3) (add 3 9)";
            var ast = AstProducer.Parse(source);
            var tast = Annotator.Annotate(ast);
            var constraints = ConstraintCollector.Collect(tast);
            var result = TypeUnifier.Unify(constraints);
            var typed = HindleyMilner.Infer(ast);
            return;
        }

        [TestMethod]
        public void HMTest()
        {
            string source = "(def a 5) (def b 6) (add a b)";
            var ast = AstProducer.Parse(source);
            var results = HindleyMilner.Infer(ast);
            return;
        }
    }
}
