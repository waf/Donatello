using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Antlr4.Runtime.Tree;
using Microsoft.CodeAnalysis;

namespace DotNetLisp
{
    internal static class BuiltInFunctions
    {
        public static void AddBuiltInVariables(Scope globalScope)
        {
            globalScope.Variables["true"] = LiteralExpression(SyntaxKind.TrueLiteralExpression);
            globalScope.Variables["false"] = LiteralExpression(SyntaxKind.FalseLiteralExpression);
        }

        internal static ExpressionSyntax Run(
            IParseTreeVisitor<ExpressionSyntax> visitor,
            IList<IParseTree> children)
        {
            switch (children[0].GetText())
            {
                case "def": return Def(visitor, children);
                case "if": return If(visitor, children);
                case "+": return Add(visitor, children);
                default:
                    return null;
            }
        }

        private static ExpressionSyntax If(
            IParseTreeVisitor<ExpressionSyntax> visitor,
            IList<IParseTree> children)
        {
            // (if condition then-statement else-statement)
            var condition = visitor.Visit(children[1]);
            var thenStatement = visitor.Visit(children[2]);
            var elseStatement = visitor.Visit(children[3]);
            return ConditionalExpression(condition, thenStatement, elseStatement);
        }

        private static ExpressionSyntax Def(
            IParseTreeVisitor<ExpressionSyntax> visitor,
            IList<IParseTree> children)
        {
            // (def a 5)
            var name = children[1].GetText();
            var value = visitor.Visit(children[2]);
            Program.GlobalScope.Variables[name] = value;
            return value;
        }

        private static ExpressionSyntax Add(
            IParseTreeVisitor<ExpressionSyntax> visitor,
            IList<IParseTree> children)
        {
            // (+ a b c ...)
            var values = children.Skip(1).Select(child => visitor.Visit(child));
            return values.Aggregate((a, b) => BinaryExpression(SyntaxKind.AddExpression, a, b));
        }
    }
}
