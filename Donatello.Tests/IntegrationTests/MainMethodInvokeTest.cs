using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Donatello.Tests.TestHelpers;

namespace Donatello.Tests.IntegrationTests
{
    [TestClass]
    public class MainMethodInvokeTest
    {
        [TestMethod]
        public void InvokeFunction()
        {
            const string program = @"(defn getOne [] 1) (System.Console.WriteLine (getOne))";
            string result = RunProgramAndCaptureOutput(program);
            Assert.AreEqual("1\r\n", result);
        }

        [TestMethod]
        public void ReferenceVariable()
        {
            const string program = @"(def one 1) (System.Console.WriteLine one)";
            string result = RunProgramAndCaptureOutput(program);
            Assert.AreEqual("1\r\n", result);
        }
    }
}
