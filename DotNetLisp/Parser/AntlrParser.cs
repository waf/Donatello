using Antlr4.Runtime;
using DotNetLisp.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
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
        public static CSharpSyntaxNode Parse(string input, string namespaceName, string className)
        {
            var visitor = new ParseExpressionVisitor(namespaceName, className);

            using (var stream = new StringReader(input))
            {
                AntlrInputStream inputStream = new AntlrInputStream(stream);

                DotNetLispLexer lexer = new DotNetLispLexer(inputStream);
                CommonTokenStream commonTokenStream = new CommonTokenStream(lexer);
                DotNetLispParser parser = new DotNetLispParser(commonTokenStream);
                var file = parser.file();

                return visitor.Visit(file);
            }
        }
    }
}
