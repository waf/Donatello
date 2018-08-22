using System.Collections.Generic;
using System.Linq;

namespace Donatello.Ast
{
	class FileUntypedExpression : IExpression
    {
        public FileUntypedExpression(IEnumerable<IExpression> statements) =>
            Statements = statements.ToArray();

        public IReadOnlyList<IExpression> Statements { get; }
    }
}
