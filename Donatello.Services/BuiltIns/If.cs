using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.BuiltIns;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.BuiltIns
{
    internal class If : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            // (if condition then-statement else-statement)
            var condition = visitor.Visit(children[1]) as ExpressionSyntax;
            var thenStatement = visitor.Visit(children[2]) as ExpressionSyntax;
            var elseStatement = visitor.Visit(children[3]) as ExpressionSyntax;
            return ParenthesizedExpression(
                ConditionalExpression(condition, thenStatement, elseStatement)
            );
        }
    }
}