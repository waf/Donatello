using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<Expression>
    {
        public override Expression VisitSymbol([NotNull] DotNetLispParser.SymbolContext context)
        {
            string name = context.SYMBOL().GetText();

            var scope = Program.ScopeAnnotations.Get(context) ?? Program.GlobalScope;
            Expression value = null;
            bool found = scope.Variables.TryGetValue(name, out value);
            if(!found)
            {
                throw new Exception("Unknown symbol: " + name);
            }
            return value;
        }
    }
}
