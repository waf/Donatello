using Antlr4.Runtime;
using DotNetLisp.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotNetLisp.Parser
{
    static class AntlrParser
    {
        /// <summary>
        /// Use ANTLR4 and the associated visitor implementation to produce a roslyn AST
        /// </summary>
        public static CompilationUnitSyntax Parse(string input, string namespaceName, string className, string mainMethodName)
        {
            var visitor = new ParseExpressionVisitor(namespaceName, className, mainMethodName);

            using (var stream = new StringReader(input))
            {
                AntlrInputStream inputStream = new AntlrInputStream(stream);

                DotNetLispLexer lexer = new DotNetLispLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                DotNetLispParser parser = new DotNetLispParser(commonTokenStream);
                var file = parser.file();

                return visitor.Visit(file) as CompilationUnitSyntax;
            }
        }
    }
}
