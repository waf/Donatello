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
        public override CSharpSyntaxNode VisitList([NotNull] DotNetLispParser.ListContext context)
        {
            var children = context.forms().children;

            return CreateListInvocation(children);
        }

        private CSharpSyntaxNode CreateListInvocation(IList<IParseTree> children)
        {
            var builtIn = BuiltInFunctions.Run(this, children);
            if (builtIn != null)
            {
                return builtIn;
            }

            var elements = children
                .Select(child => this.Visit(child) as ExpressionSyntax)
                .ToArray();

            return BuildInvocation((dynamic)elements[0], elements.Skip(1).ToArray());
        }

        private CSharpSyntaxNode BuildInvocation(ExpressionSyntax first, ExpressionSyntax[] rest)
        {
            var staticArgs = ArgumentList(SeparatedList(rest.Select(Argument)));
            return InvocationExpression(first, staticArgs);
        }

        private CSharpSyntaxNode BuildInvocation(MemberAccessExpressionSyntax first, ExpressionSyntax[] rest)
        {
            return first.WithExpression(rest[0]);
        }

        private CSharpSyntaxNode BuildInvocation(InvocationExpressionSyntax first, ExpressionSyntax[] rest)
        {
            var instance = rest[0];
            var methodArguments = rest.Skip(1).Select(Argument).ToArray();
            return first
                .WithExpression((first.Expression as MemberAccessExpressionSyntax).WithExpression(instance))
                .WithArgumentList(ArgumentList(SeparatedList(methodArguments)));
        }
    }
}
