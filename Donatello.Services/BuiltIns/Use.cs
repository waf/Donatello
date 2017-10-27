using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    internal class Use : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var target = children[1].GetText().Split('.');
            if(target.Last() == "*")
            {
                return UsingDirective(
                    Token(SyntaxKind.StaticKeyword),
                    null,
                    ParseName(string.Join(".", target.Take(target.Length - 1))));
            }
            return UsingDirective(ParseName(string.Join(".", target)));
        }
    }
}