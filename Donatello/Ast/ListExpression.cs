
using Donatello.TypeInference;
using System.Collections.Generic;
using System.Linq;

namespace Donatello.Ast
{
	class ListUntypedExpression : IExpression
    {
        public ListUntypedExpression(IEnumerable<IExpression> elements)
        {
            Elements = elements.ToArray();
        }

        public IReadOnlyList<IExpression> Elements { get; }
    }

    class ListExpression : ITypedExpression
    {
        public ListExpression(IReadOnlyList<ITypedExpression> elements, IType type)
        {
            Elements = elements;
            Type = type;
        }

        public IReadOnlyList<ITypedExpression> Elements { get; }
        public IType Type { get; set; }
        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

    class VectorUntypedExpression : IExpression
    {
        public VectorUntypedExpression(IEnumerable<IExpression> elements)
        {
            Elements = elements.ToArray();
        }

        public IReadOnlyList<IExpression> Elements { get; }
    }

    class VectorExpression : ITypedExpression
    {
        public VectorExpression(IEnumerable<ITypedExpression> elements, IType type)
        {
            Elements = elements;
            Type = type;
        }

        public IEnumerable<ITypedExpression> Elements { get; }
        public IType Type { get; set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

    class SetUntypedExpression : IExpression
    {
        public SetUntypedExpression(IEnumerable<IExpression> elements) =>
            Elements = elements.ToArray();

        public IReadOnlyList<IExpression> Elements { get; }
    }

    class SetExpression : ITypedExpression
    {
        public SetExpression(IEnumerable<ITypedExpression> elements, IType type)
        {
            Elements = elements;
            Type = type;
        }

        public IEnumerable<ITypedExpression> Elements { get; }
        public IType Type { get; set; }
        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
