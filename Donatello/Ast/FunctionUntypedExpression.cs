using System;
using System.Collections.Generic;
using Donatello.Ast;

using System.Linq;
using Donatello.TypeInference;

namespace Donatello.Ast
{
    class FunctionUntypedExpression : IExpression
    {
        public string Name { get; set; }
        public IReadOnlyList<SymbolUntypedExpression> Arguments { get; set; }
        public IReadOnlyList<IExpression> Body { get; set; }

        public FunctionUntypedExpression(string name, IEnumerable<SymbolUntypedExpression> arguments, IEnumerable<IExpression> body)
        {
            this.Name = name;
            this.Arguments = arguments.ToArray();
            this.Body = body.ToArray();
        }
    }
}