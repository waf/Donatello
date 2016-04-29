using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotNetLisp
{
    internal class Scope
    {
        public IDictionary<string, ExpressionSyntax> Variables { get; }
            = new Dictionary<string, ExpressionSyntax>();

        //TODO: scope resolution chain
    }
}