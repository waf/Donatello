
using Donatello.TypeInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class BooleanExpression : ITypedExpression
    {
        public BooleanExpression(string value) =>
            Value = bool.Parse(value);

        public bool Value { get; }
        public IType Type { get; set; } = new ConcreteType(typeof(bool));

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
