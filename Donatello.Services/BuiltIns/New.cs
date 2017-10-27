using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    internal class New : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var type = children[1].GetText();
            var constructorParameters = children
                .Skip(2)
                .Select(child => Argument(visitor.Visit(child) as ExpressionSyntax));
            return ObjectCreationExpression(ParseTypeName(type))
                .WithArgumentList(ArgumentList(SeparatedList(constructorParameters)));
        }
    }
}