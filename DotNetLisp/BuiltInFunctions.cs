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

        internal static CSharpSyntaxNode Run(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            switch (children[0].GetText())
            {
                case "def": return Def(visitor, children);
                case "fun": return Fun(visitor, children);
                case "if": return If(visitor, children);
                case "+": return Add(visitor, children);
                default:
                    return null;
            }
        }

        private static CSharpSyntaxNode Fun(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            var methodName = children[1].GetText();
            var returnType = children[4].GetText();
            var body = visitor.Visit(children[5]);
            return MethodDeclaration(ParseTypeName(returnType), methodName)
                        .WithBody(Block(ReturnStatement(body as ExpressionSyntax)));
        }

        private static CSharpSyntaxNode If(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (if condition then-statement else-statement)
            var condition = visitor.Visit(children[1]) as ExpressionSyntax;
            var thenStatement = visitor.Visit(children[2]) as ExpressionSyntax;
            var elseStatement = visitor.Visit(children[3]) as ExpressionSyntax;
            return ConditionalExpression(condition, thenStatement, elseStatement);
        }

        private static CSharpSyntaxNode Def(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (def a 5)
            var name = children[1].GetText();
            var value = visitor.Visit(children[2]);
            Program.GlobalScope.Variables[name] = value;
            return value;
        }

        private static CSharpSyntaxNode Add(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            // (+ a b c ...)
            var values = children.Skip(1).Select(child => visitor.Visit(child) as ExpressionSyntax);
            return values.Aggregate((a, b) => BinaryExpression(SyntaxKind.AddExpression, a, b));
        }
    }
}
