using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using static Donatello.Services.Antlr.Generated.DonatelloParser;
using Donatello.Services.Util;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    class Defn : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var methodName = children[1].GetText();
            var parameters = children[2].GetChild(0);

            var parameterList = parameters.As<VectorContext>().form().InPairs((name, type) =>
            {
                return Parameter(Identifier(name.GetText()))
                    .WithType(visitor.Visit(type) as TypeSyntax);
            });

            var returnType = visitor.Visit(children[3]) as TypeSyntax;
            var statements = children.Skip(4).Select(statement => visitor.Visit(statement)).ToArray();
            int finalElement = statements.Length - 1;
            var body = statements
                .Select((expression, index) =>
                            index == finalElement && !IsVoid(returnType) ?
                            ReturnStatement(expression as ExpressionSyntax) :
                            ExpressionStatement(expression as ExpressionSyntax) as StatementSyntax)
                .ToArray();
            return MethodDeclaration(returnType, methodName)
                    .WithParameterList(ParameterList(SeparatedList(parameterList)))
                    .WithBody(Block(body));
        }

        private static bool IsVoid(TypeSyntax returnType)
        {
            var voidType = returnType as PredefinedTypeSyntax;
            return voidType != null && voidType.Keyword.Kind() == SyntaxKind.VoidKeyword;
        }
    }
}
