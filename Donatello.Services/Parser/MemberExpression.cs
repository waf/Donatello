using Donatello.Services.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Misc;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitFieldAccess([NotNull] DonatelloParser.FieldAccessContext context)
        {
            string name = context.children[0].GetText();
            return MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                IdentifierName(string.Empty),
                IdentifierName(name.Substring(1)));
        }

        public override CSharpSyntaxNode VisitMethodAccess([NotNull] DonatelloParser.MethodAccessContext context)
        {
            string name = context.children[0].GetText();
            return InvocationExpression(
                MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                    IdentifierName(string.Empty),
                    IdentifierName(name.Substring(1))),
                ArgumentList());
        }
    }
}
