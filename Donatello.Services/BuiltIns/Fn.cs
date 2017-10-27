using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.BuiltIns;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Donatello.Services.Util;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    internal class Fn : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
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
    }
}