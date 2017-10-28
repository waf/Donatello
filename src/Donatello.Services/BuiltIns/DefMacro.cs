using System.Collections.Generic;
using Antlr4.Runtime.Tree;
using Donatello.Services.BuiltIns;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.BuiltIns
{
    internal class DefMacro : IBuiltIn
    {
        private static Defn Defn = new Defn();

        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            var result = Defn.Invoke(visitor, children) as MethodDeclarationSyntax;
            Macros.AddMacro(result, visitor.NamespaceName, visitor.ClassName);
            return result; // return the macro as a function so other programs can reference it.
        }
    }
}