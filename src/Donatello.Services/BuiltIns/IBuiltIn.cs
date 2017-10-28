using Antlr4.Runtime.Tree;
using Donatello.Services.Parser;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace Donatello.Services.BuiltIns
{
    interface IBuiltIn
    {
        CSharpSyntaxNode Invoke(ParseExpressionVisitor visitor, IList<IParseTree> children);
    }
}
