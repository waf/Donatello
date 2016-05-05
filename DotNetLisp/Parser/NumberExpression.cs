using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitNumber([NotNull] DotNetLispParser.NumberContext context)
        {
            var numberText = context.GetText();
            var number = long.Parse(numberText);
            return LiteralExpression(SyntaxKind.NumericLiteralExpression, Literal(number));
        }
    }
}
