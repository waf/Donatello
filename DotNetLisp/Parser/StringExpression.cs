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
        public override Expression VisitString([NotNull] DotNetLispParser.StringContext context)
        {
            var str = context.GetText();
            str = str.Substring(1, str.Length - 2); //strip quotes
            return Expression.Constant(str);
        }
    }
}
