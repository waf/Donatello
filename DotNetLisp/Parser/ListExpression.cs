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

            /*
            if(children == null)
            {
                return Expression.New(typeof(List<object>));
            }

            if(children[0].GetText() == "def")
            {
                return DefineVariable(children);
            }
            if (children[0].GetText() == "if")
            {
                return DefineLet(children);
            }
            */

            ExpressionSyntax[] elements = children
                .Select(child => this.Visit(child))
                .ToArray();

            /*
            if(elements[0].NodeType != ExpressionType.Lambda)
            {
                throw new Exception("First element of a list must be a function");
            }
            */

            var arguments = SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(
                elements.Skip(1).Select(element => SyntaxFactory.Argument(element))));
            return SyntaxFactory.InvocationExpression(elements.First(), arguments);
        }

        /*
        private Expression DefineLet(IList<IParseTree> children)
        {
            // (if condition then-statement else-statement)
            var condition = this.Visit(children[1]);
            var thenStatement = this.Visit(children[2]);
            var elseStatement = this.Visit(children[3]);
            return Expression.Condition(condition, thenStatement, elseStatement);
        }

        private Expression DefineVariable(IList<IParseTree> children)
        {
            // (def a 5)
            var name = children[1].GetText();
            var value = this.Visit(children[2]);
            Program.GlobalScope.Variables[name] = value;
            return value;
        }
        */
    }
}
