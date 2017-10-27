using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Donatello.StandardLibrary;
using System.Linq;
using Donatello.Services.Util;
using static Donatello.Services.Antlr.Generated.DonatelloParser;

namespace Donatello.Services.BuiltIns
{
    internal class Let : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var bindings = children[1].GetChild(0);
            var expressions = children.Skip(2).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = expressions.Length - 1;

            var variables = bindings.As<VectorContext>().form().InPairs((name, value) =>
            {
                return LocalDeclarationStatement(
                        VariableDeclaration(IdentifierName("var"))
                        .WithVariables(SingletonSeparatedList(
                            VariableDeclarator(Identifier(name.GetText()))
                            .WithInitializer(EqualsValueClause(visitor.Visit(value) as ExpressionSyntax)))));
            });

            var statements = expressions
                .Select((expression, index) =>
                            index == finalElement ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax);

            var lambda = ParenthesizedLambdaExpression(Block(variables.Union(statements)));

            return InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(Constructors)),
                        IdentifierName(nameof(Constructors.CreateLet))))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(lambda))));
        }
    }
}