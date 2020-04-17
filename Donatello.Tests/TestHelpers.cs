using Antlr4.Runtime;
using Donatello.Ast;
using Donatello.Parser;
using Donatello.Parser.Generated;
using Donatello.Services;
using Donatello.TypeInference;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Donatello.Tests
{
	static class TestHelpers
    {
        const string assemblyName = "Output";
        const string className = "Program";

        /// <summary>
        /// Parse a program into an syntax tree node.
        /// </summary>
        public static TNode Parse<TNode>(string program)
            where TNode : ParserRuleContext
        {
            AntlrInputStream inputStream = new AntlrInputStream(program);
            DonatelloLexer lexer = new DonatelloLexer(inputStream);
            CommonTokenStream tokenStream = new CommonTokenStream(lexer);
            DonatelloParser parser = new DonatelloParser(tokenStream);
            var parsed = parser.file()
                .GetChild(0)  // first line of file
                .GetChild(0); // child of Form

            Assert.IsInstanceOfType(parsed, typeof(TNode));

            return (TNode)parsed;
        }

        public static Assembly CompileProgram(string program)
        {
            var ast = AstProducer.Parse(program);
            var typedAst = HindleyMilner.Infer(ast);
            var assembly = Compiler.BuildAssembly(typedAst, assemblyName, className);
            assembly.Save("output.exe");
            return assembly;
        }

        public static string RunProgramAndCaptureOutput(string program, string entryPointMethod = "Main")
        {
            var assembly = CompileProgram(program);
            return CaptureOutputFromMethod(assembly, entryPointMethod);
        }

        private static string CaptureOutputFromMethod(Assembly assembly, string method)
        {
            lock (assemblyName) // lock because we're changing static Console.Out
            {
                using (var writer = new StringWriter())
                {
                    var originalOut = Console.Out;
                    Console.SetOut(writer);
                    try
                    {
                        assembly
                            .GetType(className)
                            .GetMethod(method)
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

        public static IEnumerable<TFirst> Find<TFirst>(this ITypedExpression expr)
        {
            foreach (var child in GetChildren(expr))
            {
                if(child.Count >= 1
                    && child[0] is TFirst first)
                {
                    yield return first;
                }
            }
        }

        public static IEnumerable<(TFirst, TSecond, TThird)> Find<TFirst, TSecond, TThird>(this ITypedExpression expr)
        {
            foreach (var child in GetChildren(expr))
            {
                if(child.Count >= 3
                    && child[0] is TFirst first
                    && child[1] is TSecond second
                    && child[2] is TThird third)
                {
                    yield return (first, second, third);
                }
            }
        }

        private static IEnumerable<IReadOnlyList<ITypedExpression>> GetChildren(ITypedExpression expr)
        {
            switch (expr)
            {
                case ListExpression list:
                    return new[] { list.Elements }
                        .Concat(list.Elements.SelectMany(GetChildren));
                case DefExpression def:
                    return new[] { new[] { def } }
                        .Concat(GetChildren(def.Body));
                case FileExpression file:
                    return 
                        file.Statements.SelectMany(GetChildren);
                default:
                    return Enumerable.Empty<IReadOnlyList<ITypedExpression>>();
            }
        }
    }
}
