
using Donatello.TypeInference;

namespace Donatello.Ast
{
	class SymbolExpression : ITypedExpression
    {
        public SymbolExpression(string name, IType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }
        public IType Type { get; set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
