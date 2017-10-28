using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Donatello.Services.BuiltIns
{
    class Def : IBuiltIn
    {
        public CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children)
        {
            // (def a:int 5)
            var name = children[1].GetText();
            var type = visitor.Visit(children[2]) as TypeSyntax;
            var value = visitor.Visit(children[3]) as ExpressionSyntax;
            return FieldDeclaration(
                VariableDeclaration(type, SingletonSeparatedList(
                    VariableDeclarator(name).WithInitializer(EqualsValueClause(value)))));
        }
    }
}
