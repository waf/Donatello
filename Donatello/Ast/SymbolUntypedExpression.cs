
using Donatello.TypeInference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class SymbolUntypedExpression : IExpression
    {
        public SymbolUntypedExpression(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
