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

            var access = elements[0] as MemberAccessExpressionSyntax;
            if(access != null && access.Expression.GetText().ToString() == string.Empty)
            {
                var instanceInvocation = access.WithExpression(elements[1]);
                var instanceArgs = ArgumentList(SeparatedList(elements.Skip(2).Select(Argument)));
                return InvocationExpression(instanceInvocation, instanceArgs);
            }
            var staticInvocation = elements[0];
            var staticArgs = ArgumentList(SeparatedList(elements.Skip(1).Select(Argument)));
            return InvocationExpression(staticInvocation, staticArgs);
        }
    }
}
