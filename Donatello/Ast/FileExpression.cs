
using Donatello.TypeInference;
using System.Collections.Generic;
using System.Linq;

namespace Donatello.Ast
{
	class FileExpression : ITypedExpression
    {
        public FileExpression(IEnumerable<ITypedExpression> statements) =>
            Statements = statements.ToArray();

        public IReadOnlyList<ITypedExpression> Statements { get; }
        public IType Type { get; set; }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
        public T Accept<T>(IVisitor<T> visitor) => visitor.Visit(this);
    }
}
