﻿using Antlr4.Runtime;
using Donatello.Antlr.Generated;
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
        public static CompilationUnitSyntax Parse(string input, string namespaceName, string className, string mainMethodName = null)
        {
            var visitor = new ParseExpressionVisitor(namespaceName, className, mainMethodName);

            using (var stream = new StringReader(input))
            {
                var inputStream = new AntlrInputStream(stream);

                var lexer = new DonatelloLexer(inputStream);
                var commonTokenStream = new CommonTokenStream(lexer);
                var parser = new DonatelloParser(commonTokenStream);
                var file = parser.file();

                return visitor.Visit(file) as CompilationUnitSyntax;
            }
        }
    }
}