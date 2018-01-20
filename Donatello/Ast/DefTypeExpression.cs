using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Donatello.TypeInference;

namespace Donatello.Ast
{
    class DefTypeExpression : ITypedExpression
    {
        public DefTypeExpression(
            string identifier,
            IReadOnlyList<Property> properties)
        {
            Identifier = identifier;
            Properties = properties;
        }

        public string Identifier { get; set; }
        public IReadOnlyList<Property> Properties { get; set; }

        public IType Type { get; } = new ConcreteType(typeof(void));

        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }

    class Property
    {
        public Property(string identifier, string type)
        {
            this.Identifier = identifier.TrimStart('-');
            this.Type = type;
        }

        public string Identifier { get; set; }
        public string Type { get; set; }
    }
}
