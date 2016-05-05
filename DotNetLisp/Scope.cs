using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DotNetLisp
{
    internal class Scope
    {
        public IDictionary<string, CSharpSyntaxNode> Variables { get; }
            = new Dictionary<string, CSharpSyntaxNode>();

        //TODO: scope resolution chain
    }
}