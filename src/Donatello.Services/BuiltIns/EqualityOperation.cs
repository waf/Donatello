using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Donatello.StandardLibrary;

namespace Donatello.Services.BuiltIns
{
    class EqualityOperation : IBuiltIn
    {
        private SyntaxKind equalityOperator;

        public EqualityOperation(SyntaxKind equalityOperator)
        {
            this.equalityOperator = equalityOperator;
        }

        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var values = children
                .Skip(1)
                .Select(child => visitor.Visit(child) as ExpressionSyntax)
                .ToArray();

            // optimize for common scenario
            if(values.Length == 2)
            {
                return ParenthesizedExpression(
                    BinaryExpression(equalityOperator, values[0], values[1])
                );
            }

            // create a var declaration for each operand.
            var variableNames = Enumerable.Range(1, int.MaxValue)
                                 .Select(n => "operand" + n)
                                 .Take(values.Length)
                                 .ToArray();
            StatementSyntax[] variableDeclarations = variableNames
                .Zip(values, (variable, value) =>
                    LocalDeclarationStatement(
                        VariableDeclaration(IdentifierName("var"))
                            .WithVariables(SingletonSeparatedList(
                                VariableDeclarator(Identifier(variable))
                                .WithInitializer(EqualsValueClause(value))))))
                .ToArray();

            StatementSyntax andStatement = ReturnStatement(
                // pairwise list visit to create [operand1 < operand2, operand2 < operand3, operand3 < operand4]
                variableNames.Zip(variableNames.Skip(1),
                    (a, b) => BinaryExpression(equalityOperator, IdentifierName(a), IdentifierName(b)))
                // 'and' the comparisons together
                .Aggregate((a, b) => BinaryExpression(SyntaxKind.LogicalAndExpression, a, b)));

            // execute let expression
            var lambda = ParenthesizedLambdaExpression(Block(variableDeclarations.Union(new[] { andStatement })));
            return ParenthesizedExpression(
                InvocationExpression(
                    MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        IdentifierName(nameof(Constructors)),
                        IdentifierName(nameof(Constructors.CreateLet))))
                    .WithArgumentList(ArgumentList(SingletonSeparatedList(Argument(lambda))))
            );
        }
    }
}
