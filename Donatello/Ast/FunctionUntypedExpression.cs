using System;
using System.Collections.Generic;
using Donatello.Ast;

using System.Linq;
using Donatello.TypeInference;

namespace Donatello.Ast
{
    class FunctionUntypedExpression : IExpression
    {
        public SymbolUntypedExpression Symbol { get; set; }
        public IReadOnlyList<SymbolUntypedExpression> Arguments { get; set; }
        public IReadOnlyList<IExpression> Body { get; set; }

        public FunctionUntypedExpression(SymbolUntypedExpression symbol, IEnumerable<SymbolUntypedExpression> arguments, IEnumerable<IExpression> body)
        {
            this.Symbol = symbol;
            this.Arguments = arguments.ToArray();
            this.Body = body.ToArray();
        }
    }
}