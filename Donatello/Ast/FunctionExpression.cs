using System.Collections.Generic;
using Donatello.TypeInference;

namespace Donatello.Ast
{
	class FunctionExpression : ITypedExpression
    {
        public FunctionExpression(SymbolExpression symbol, IReadOnlyList<SymbolExpression> arguments, IReadOnlyList<ITypedExpression> body, IType type)
        {
            Symbol = symbol;
            Arguments = arguments;
            Body = body;
            Type = (FunctionType)type;
        }

        public SymbolExpression Symbol { get; set; }
        public IReadOnlyList<SymbolExpression> Arguments { get; set; }
        public IReadOnlyList<ITypedExpression> Body { get; set; }

        public FunctionType Type { get; set; }
        IType ITypedExpression.Type => this.Type;

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}