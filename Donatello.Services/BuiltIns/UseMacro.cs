using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.BuiltIns
{
    internal class UseMacro : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var path = children[1].GetText();
            Macros.ResolveMacro(path);
            return EmptyStatement();
        }
    }
}