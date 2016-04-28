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
            globalScope.Variables["+"] = Add<long>();
            globalScope.Variables["str"] = (Expression<Func<long, string>>)(obj => obj.ToString());
            globalScope.Variables["bool"] = (Expression<Func<long, bool>>)(i => i != 0);
            globalScope.Variables["true"] = Expression.Constant(true);
            globalScope.Variables["false"] = Expression.Constant(false);
            globalScope.Variables["not"] = (Expression<Func<bool, bool>>)(b => !b);
            globalScope.Variables["print"] = (Expression<Func<string, string>>)(str => Print(str));
        }

        private static string Print(string str)
        {
            Console.WriteLine(str);
            return str;
        }

        public static Expression Add<T>()
        {
            var x = Expression.Parameter(typeof(T), "x");
            var y = Expression.Parameter(typeof(T), "y");
            var body = Expression.Add(x, y);
            return Expression.Lambda<Func<T, T, T>>(body, x, y);
        }
    }
}
