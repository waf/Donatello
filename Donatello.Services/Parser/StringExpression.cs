using Antlr4.Runtime.Misc;
using Donatello.Services.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Services.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitString([NotNull] DonatelloParser.StringContext context)
        {
            var str = context.GetText();
            str = str.Substring(1, str.Length - 2); //strip quotes
            return LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(str));
        }
    }
}
