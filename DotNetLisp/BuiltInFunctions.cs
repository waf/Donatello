using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DotNetLisp
{
    internal class BuiltInFunctions
    {
        public static void AddBuiltinFunctions(Scope globalScope)
        {
            //globalScope.Variables["+"] = Add;
            //globalScope.Variables["foo"] = _ => SyntaxFactory.LiteralExpression(SyntaxKind.NumericLiteralExpression, SyntaxFactory.Literal(5));
            /*
            globalScope.Variables["str"] = (Expression<Func<long, string>>)(obj => obj.ToString());
            globalScope.Variables["bool"] = (Expression<Func<long, bool>>)(i => i != 0);
            globalScope.Variables["true"] = Expression.Constant(true);
            globalScope.Variables["false"] = Expression.Constant(false);
            globalScope.Variables["not"] = (Expression<Func<bool, bool>>)(b => !b);
            globalScope.Variables["print"] = (Expression<Func<string, string>>)(str => Print(str));
            */
        }

        private static string Print(string str)
        {
            Console.WriteLine(str);
            return str;
        }

        public static ExpressionSyntax Add(ExpressionSyntax[] args)
        {
            return SyntaxFactory.BinaryExpression(SyntaxKind.AddExpression, args[0], args[1]);
        }
    }
}
