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
using DotNetLisp.StandardLibrary;
using DotNetLisp.Util;

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
                case "defn": return Defn(visitor, children);
                case "fn": return Fn(visitor, children);
                case "if": return If(visitor, children);
                case "let": return Let(visitor, children);
                case "use": return Use(visitor, children);
                case "+": return Add(visitor, children);
                default:
                    return null;
            }
        }

        private static CSharpSyntaxNode Fn(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            /*
                (fn [a b] (+ a b))
             */

            var bindings = children[1].GetChild(0).Children().ToList();
            var parameters = bindings.Skip(1).Take(bindings.Count - 2)
                                    .Select(var => Parameter(Identifier(var.GetText())));
            var expressions = children.Skip(2).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = expressions.Length - 1;
            var statements = expressions
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();
            return ParenthesizedLambdaExpression(Block(statements))
                    .WithParameterList(ParameterList(SeparatedList(parameters)));
        }

        private static CSharpSyntaxNode Let(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var bindings = children[1].GetChild(0);
            var expressions = children.Skip(2).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = expressions.Length - 1;
            var statements = expressions
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();

            List<StatementSyntax> variables = PairwiseListVisit<StatementSyntax>(bindings, (name, value) =>
            {
                return LocalDeclarationStatement(
                        VariableDeclaration(IdentifierName("var"))
                        .WithVariables(SingletonSeparatedList(
                            VariableDeclarator(Identifier(name.GetText()))
                            .WithInitializer(EqualsValueClause(visitor.Visit(value) as ExpressionSyntax)))));
            });

            var lambda = ParenthesizedLambdaExpression(Block(variables.Union(statements)));

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(Constructors)),
                        IdentifierName(nameof(Constructors.CreateLet))))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(lambda))));
        }

        private static List<T> PairwiseListVisit<T>(IParseTree tree, Func<IParseTree, IParseTree, T> pairwiseOperation)
        {
            var list = new List<T>();
            for (int i = 1; i < tree.ChildCount - 1; i += 2)
            {
                list.Add(
                    pairwiseOperation(tree.GetChild(i), tree.GetChild(i + 1))
                );
            }

            return list;
        }

        private static CSharpSyntaxNode Use(IParseTreeVisitor<CSharpSyntaxNode> visitor, IList<IParseTree> children)
        {
            var target = children[1].GetText().Split('.');
            if(target.Last() == "*")
            {
                return UsingDirective(
                    Token(SyntaxKind.StaticKeyword),
                    null,
                    ParseName(string.Join(".", target.Take(target.Length - 1))));
            }
            return UsingDirective(
                NameEquals(IdentifierName(target.Last()),
                Token(SyntaxKind.EqualsToken)),
                ParseName(string.Join(".", target)));
        }

        private static CSharpSyntaxNode Defn(
            IParseTreeVisitor<CSharpSyntaxNode> visitor,
            IList<IParseTree> children)
        {
            var methodName = children[1].GetText();
            var parameters = children[2].GetChild(0);

            IList<ParameterSyntax> parameterList = PairwiseListVisit(parameters, (name, type) =>
            {
                return Parameter(Identifier(name.GetText()))
                    .WithType(visitor.Visit(type) as TypeSyntax);
            });

            var returnType = visitor.Visit(children[3]) as TypeSyntax;
            var statements = children.Skip(4).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = statements.Length - 1;
            var body = statements
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();
            return MethodDeclaration(returnType, methodName)
                    .WithParameterList(ParameterList(SeparatedList(parameterList)))
                    .WithBody(Block(body));
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
