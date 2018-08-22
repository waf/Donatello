
using Donatello.TypeInference;

namespace Donatello.Ast
{
	interface ILiteralExpression
    {
        object Value { get; }
    }

    class LongExpression : ITypedExpression, ILiteralExpression
    {
        public LongExpression(string value) =>
            Value = long.Parse(value);

        public long Value { get; }
        public IType Type { get; set; } = new ConcreteType(typeof(long));
        object ILiteralExpression.Value => Value;

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

}
