using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitVector([NotNull] DotNetLispParser.VectorContext context)
        {
            var children = context.form();

            var elements = children
                .Select(child => this.Visit(child) as ExpressionSyntax)
                .ToArray();

            return ImplicitArrayCreationExpression(InitializerExpression(SyntaxKind.ArrayInitializerExpression, SeparatedList(elements)));
        }
    }
}
