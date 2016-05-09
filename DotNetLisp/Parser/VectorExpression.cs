using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Newtonsoft.Json;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitVector([NotNull] DotNetLispParser.VectorContext context)
        {
            var children = context.form();

            var arguments = children
                .Select(child => Argument(Visit(child) as ExpressionSyntax))
                .ToArray();

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(ImmutableArray)),
                        IdentifierName(nameof(ImmutableArray.Create))))
                    .WithArgumentList(ArgumentList(SeparatedList(arguments)));
        }
    }
}
