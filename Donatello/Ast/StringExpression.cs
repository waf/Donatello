
using Donatello.TypeInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class StringExpression : ITypedExpression, ILiteralExpression
    {
        public StringExpression(string value) =>
            Value = value;

        public string Value { get; }
        public IType Type { get; set; } = new ConcreteType(typeof(string));
        object ILiteralExpression.Value => Value;

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
