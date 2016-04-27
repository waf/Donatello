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
