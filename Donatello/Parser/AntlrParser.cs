using Antlr4.Runtime;
using Donatello.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Donatello.Parser
{
    static class AntlrParser
    {
        /// <summary>
        /// Use ANTLR4 and the associated visitor implementation to produce a roslyn AST
        /// </summary>
        public static CSharpSyntaxNode ParseAsRepl(string input)
        {
            var visitor = new ParseExpressionVisitor(repl: true);
            return Parse(input, visitor);
        }

        /// <summary>
        /// Use ANTLR4 and the associated visitor implementation to produce a roslyn AST
        /// </summary>
        public static CSharpSyntaxNode ParseAsClass(string input, string namespaceName, string className, string mainMethodName = null)
        {
            var visitor = new ParseExpressionVisitor(namespaceName, className, mainMethodName);
            return Parse(input, visitor);
        }

        private static CSharpSyntaxNode Parse(string input, ParseExpressionVisitor visitor)
        {
            using (var stream = new StringReader(input))
            {
                var inputStream = new AntlrInputStream(stream);

                var lexer = new DonatelloLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new DonatelloParser(commonTokenStream);
                var file = parser.file();

                return visitor.Visit(file);
            }
        }
    }
}
