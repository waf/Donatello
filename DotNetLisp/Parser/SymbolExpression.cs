using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        static IDictionary<string, CSharpSyntaxNode> BuiltIns = new Dictionary<string, CSharpSyntaxNode>
        {
            { "true", LiteralExpression(SyntaxKind.TrueLiteralExpression) },
            { "false", LiteralExpression(SyntaxKind.FalseLiteralExpression) },
            { "this", ThisExpression()}
        };

        public override CSharpSyntaxNode VisitSymbol([NotNull] DotNetLispParser.SymbolContext context)
        {
            string name = context.SYMBOL().GetText();

            CSharpSyntaxNode builtIn = null;
            if(BuiltIns.TryGetValue(name, out builtIn))
            {
                return builtIn;
            }

            var parts = name.Split('.');

            ExpressionSyntax simpleAccess = IdentifierName(parts.First());
            if(parts.Length == 1)
            {
                return simpleAccess;
            }

            var chainedAccess = parts.Skip(1).Aggregate(simpleAccess, 
                (access, token) => MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, access, IdentifierName(token)));

            return chainedAccess;
        }
    }
}
