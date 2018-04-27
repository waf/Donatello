
using Donatello.TypeInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class FloatExpression : ITypedExpression, ILiteralExpression
    {
        public FloatExpression(string value) =>
            Value = float.Parse(value);

        public float Value { get; }
        public IType Type { get; set; } = new ConcreteType(typeof(float));
        object ILiteralExpression.Value => Value;

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
