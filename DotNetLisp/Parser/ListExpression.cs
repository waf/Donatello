using Antlr4.Runtime.Misc;
using DotNetLisp.Antlr.Generated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp.Parser
{
    public partial class ParseExpressionVisitor : DotNetLispBaseVisitor<Expression>
    {
        private static MethodInfo[] createMethods;
        static ParseExpressionVisitor()
        {
            createMethods = typeof(Tuple).GetMethods(BindingFlags.Public | BindingFlags.Static);
        }

        public override Expression VisitList([NotNull] DotNetLispParser.ListContext context)
        {
            Expression[] elements = context.forms().children
                .Select(child => this.Visit(child))
                .ToArray();

            if(elements[0].NodeType != ExpressionType.Lambda)
            {
                throw new Exception("First element of a list must be a function");
            }

            return Expression.Invoke(elements.First(), elements.Skip(1));
        }
    }
}
