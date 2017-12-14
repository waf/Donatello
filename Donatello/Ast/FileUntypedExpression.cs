using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class FileUntypedExpression : IExpression
    {
        public FileUntypedExpression(IEnumerable<IExpression> statements) =>
            Statements = statements.ToArray();

        public IReadOnlyList<IExpression> Statements { get; }
    }
}
