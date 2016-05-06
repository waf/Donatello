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
            var parameters = children[2].GetChild(0);
            var parameterList = new List<ParameterSyntax>();
            for(int i = 1; i < parameters.ChildCount - 1; i += 2) //select every two pairs, skipping "[" and "]"
            {
                parameterList.Add(
                    Parameter(Identifier(parameters.GetChild(i).GetText()))
                        .WithType(visitor.Visit(parameters.GetChild(i + 1)) as TypeSyntax));
            }
            var returnType = visitor.Visit(children[3]) as TypeSyntax;
            var body = visitor.Visit(children[4]);
            return MethodDeclaration(returnType, methodName)
                    .AddModifiers(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.StaticKeyword))
                    .WithParameterList(ParameterList(SeparatedList(parameterList)))
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
            // (def a:int 5)
            var name = children[1].GetText();
            var type = visitor.Visit(children[2]) as TypeSyntax;
            var value = visitor.Visit(children[3]) as ExpressionSyntax;
            return FieldDeclaration(
                VariableDeclaration(type, SingletonSeparatedList(
                    VariableDeclarator(name).WithInitializer(EqualsValueClause(value)))));
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
