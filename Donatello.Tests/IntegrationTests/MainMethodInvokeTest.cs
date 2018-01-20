using Donatello.Parser;
using Donatello.TypeInference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Tests.IntegrationTests
{
    [TestClass]
    public class MainMethodInvokeTest
    {
        [TestMethod]
        public void InvokeWithFunction()
        {
            const string program = @"(defn getOne [] 1) (System.Console.WriteLine (getOne))";
            string result = RunProgramAndCaptureOutput(program);
            Assert.AreEqual("1\r\n", result);
        }

        private static string RunProgramAndCaptureOutput(string program, string entryPointMethod = "Main")
        {
            const string assemblyName = "Output";
            const string className = "Program";
            var ast = AstProducer.Parse(program);
            var typedAst = HindleyMilner.Infer(ast);
            var assembly = Compiler.BuildAssembly(typedAst, assemblyName, className);

            using (var writer = new StringWriter())
            {
                var originalOut = Console.Out;
                Console.SetOut(writer);
                try
                {
                    assembly
                        .GetType(className)
                        .GetMethod(entryPointMethod)
                        .Invoke(null, new object[] { new string[0] });
                }
                finally
                {
                    Console.SetOut(originalOut);
                }
                writer.Flush();
                return writer.GetStringBuilder().ToString();
            }
        }
    }
}
