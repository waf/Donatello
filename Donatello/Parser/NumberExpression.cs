using Antlr4.Runtime.Misc;
using Donatello.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitNumber([NotNull] DonatelloParser.NumberContext context)
        {
            var numberText = context.GetText();
            var number = int.Parse(numberText);
            return LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(number));
        }
    }
}
