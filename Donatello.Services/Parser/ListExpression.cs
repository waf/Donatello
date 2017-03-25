using Antlr4.Runtime.Misc;
using Donatello.Services.Antlr.Generated;
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

namespace Donatello.Services.Parser
{
    public partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        public override CSharpSyntaxNode VisitList([NotNull] DonatelloParser.ListContext context)
        {
            var children = context.forms().children;

            return CreateListInvocation(children);
        }

        private CSharpSyntaxNode CreateListInvocation(IList<IParseTree> children)
        {
            // macro invocation
            var head = this.Visit(children[0]);
            var tail = children.Skip(1).ToList();
            if (Macros.TryRunMacro(this, head.GetText().ToString(), tail, out CSharpSyntaxNode transformed))
            {
                return transformed;
            }

            // built-in function invocation
            var builtIn = BuiltInFunctions.Run(this, children);
            if (builtIn != null)
            {
                return builtIn;
            }

            // user function invocation
            var arguments = tail
                .Select(child => this.Visit(child) as ExpressionSyntax)
                .ToArray();
            switch (head)
            {
                case MemberAccessExpressionSyntax member: return BuildInvocation(member, arguments);
                case InvocationExpressionSyntax invocation: return BuildInvocation(invocation, arguments);
                case ExpressionSyntax expression: return BuildInvocation(expression, arguments);
                default:
                    throw new InvalidOperationException($"Unexpected first element in list: {head.GetType().FullName}");
            }
        }

        private CSharpSyntaxNode BuildInvocation(ExpressionSyntax first, ExpressionSyntax[] rest)
        {
            var staticArgs = ArgumentList(SeparatedList(rest.Select(Argument)));
            return InvocationExpression(first, staticArgs);
        }

        private CSharpSyntaxNode BuildInvocation(MemberAccessExpressionSyntax first, ExpressionSyntax[] rest)
        {
            return (first.Expression.ToString() == string.Empty) ?
                first.WithExpression(rest[0]) :
                BuildInvocation(first as ExpressionSyntax, rest);
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
