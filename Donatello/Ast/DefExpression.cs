using System;
using System.Collections.Generic;
using Donatello.Ast;

using System.Linq;
using Donatello.TypeInference;

namespace Donatello.Ast
{
    class DefExpression : ITypedExpression
    {
        public DefExpression(SymbolExpression symbol, ITypedExpression body, IType type)
        {
            Symbol = symbol;
            Body = body;
            Type = type;
        }

        public SymbolExpression Symbol { get; set; }
        public ITypedExpression Body { get; set; }
        public IType Type { get; set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}