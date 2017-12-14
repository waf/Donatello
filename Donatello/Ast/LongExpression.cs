
using Donatello.TypeInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class LongExpression : ITypedExpression
    {
        public LongExpression(string value) =>
            Value = long.Parse(value);

        public long Value { get; }
        public IType Type { get; set; } = new ConcreteType(typeof(long));

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }

}
