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
        public override Expression VisitNumber([NotNull] DotNetLispParser.NumberContext context)
        {
            var numberText = context.GetText();
            var number = long.Parse(numberText);
            return Expression.Constant(number);
        }
    }
}
