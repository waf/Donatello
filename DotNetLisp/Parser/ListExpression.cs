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

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<ExpressionSyntax>
    {
        public override ExpressionSyntax VisitList([NotNull] DotNetLispParser.ListContext context)
        {
            var children = context.forms().children;

            var builtIn = BuiltInFunctions.Run(this, children);
            if(builtIn != null)
            {
                return builtIn;
            }

            ExpressionSyntax[] elements = children
                .Select(child => this.Visit(child))
                .ToArray();

            var arguments = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(
                elements.Skip(1).Select(element => SyntaxFactory.Argument(element))));
            return SyntaxFactory.InvocationExpression(elements.First(), arguments);
        }
    }
}
