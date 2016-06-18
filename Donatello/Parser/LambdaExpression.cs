using Donatello.Antlr.Generated;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Misc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Parser
{
    partial class ParseExpressionVisitor : DonatelloBaseVisitor<CSharpSyntaxNode>
    {
        private readonly string UniqueId = "parameter_" + CreateUniqueId(12) + "_";
        private static readonly Random random = new Random();

        private static string CreateUniqueId(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override CSharpSyntaxNode VisitLambdaParameter([NotNull] DonatelloParser.LambdaParameterContext context)
        {
            var param = context.GetText();
            param = param.Replace("^", UniqueId);
            return IdentifierName(param);
        }

        public IList<IdentifierNameSyntax> GetLambdaParameters(SyntaxNode node)
        {
            var names = new List<IdentifierNameSyntax>();
            foreach(var child in node.ChildNodes())
            {
                if(child is IdentifierNameSyntax)
                {
                    names.Add(child as IdentifierNameSyntax);
                }
                else if (node is LambdaExpressionSyntax)
                {
                    continue;
                }
                names.AddRange(GetLambdaParameters(child));
            }
            return names;
        }
        public override CSharpSyntaxNode VisitLambda([NotNull] DonatelloParser.LambdaContext context)
        {
            var children = context.forms().children;

            var body = CreateListInvocation(children);

            var arguments = GetLambdaParameters(body)
                .Select(identifier => identifier.Identifier.ValueText)
                .Where(identifier => identifier.StartsWith(UniqueId, StringComparison.InvariantCulture))
                .Distinct()
                .OrderBy(i => i)
                .Select(identifier => Parameter(Identifier(identifier)));

            return ParenthesizedLambdaExpression(body)
                            .WithParameterList(
                                ParameterList(
                                    SeparatedList(
                                        arguments)));
        }
    }
}
