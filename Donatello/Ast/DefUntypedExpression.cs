﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Ast
{
    class DefUntypedExpression : IExpression
    {
        public SymbolUntypedExpression Symbol { get; set; }
        public IExpression Body { get; set; }

        public DefUntypedExpression(SymbolUntypedExpression symbol, IExpression body)
        {
            this.Symbol = symbol;
            this.Body = body;
        }
    }
}