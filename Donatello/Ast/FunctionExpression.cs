using System;
using System.Collections.Generic;
using Donatello.Ast;

using System.Linq;
using Donatello.TypeInference;

namespace Donatello.Ast
{
    class FunctionExpression : ITypedExpression
    {
        public FunctionExpression(string name, IReadOnlyList<SymbolExpression> arguments, IReadOnlyList<ITypedExpression> body, IType type)
        {
            Name = name;
            Arguments = arguments;
            Body = body;
            Type = type;
        }

        public string Name { get; set; }
        public IReadOnlyList<SymbolExpression> Arguments { get; set; }
        public IReadOnlyList<ITypedExpression> Body { get; set; }
        public IType Type { get; set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}