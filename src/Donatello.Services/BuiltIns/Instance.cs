using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.Linq;

namespace Donatello.Services.BuiltIns
{
    internal class Instance : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var baseTypesAndInterfaces = children
                .Skip(1)
                .Select(child => SimpleBaseType(ParseTypeName(child.GetText())))
                .ToArray();
            return BaseList(SeparatedList<BaseTypeSyntax>(baseTypesAndInterfaces));
        }
    }
}