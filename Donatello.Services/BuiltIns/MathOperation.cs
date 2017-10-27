using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Donatello.Services.BuiltIns
{
    class MathOperation : IBuiltIn
    {
        private SyntaxKind binaryOperation;

        public MathOperation(SyntaxKind binaryOperation)
        {
            this.binaryOperation = binaryOperation;
        }

        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var values = children.Skip(1).Select(child => visitor.Visit(child) as ExpressionSyntax);
            return ParenthesizedExpression(
                values.Aggregate((a, b) => BinaryExpression(binaryOperation, a, b))
            );
        }
    }
}
