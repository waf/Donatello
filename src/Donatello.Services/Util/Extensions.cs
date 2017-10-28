using Antlr4.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Services.Util
{
    public static class Extensions
    {
        public static IEnumerable<IParseTree> Children(this IParseTree tree)
        {
            for (int i = 0; i < tree.ChildCount; i++)
            {
                yield return tree.GetChild(i);
            }
        }

        public static string StringJoin<T>(this IEnumerable<T> enumerable, string separator)
        {
            return string.Join(separator, enumerable);
        }

        public static T As<T>(this IParseTree tree)
            where T : IParseTree
        {
            return tree is T t ? t : throw new Exception($"Could not convert {tree.GetType().Name} to {typeof(T).Name}.");
        }

        public static IEnumerable<TResult> InPairs<TSource, TResult>(
            this IEnumerable<TSource> source,
            Func<TSource, TSource, TResult> resultSelector)
        {
            TSource previous = default;

            using (var it = source.GetEnumerator())
            {
                while (it.MoveNext())
                {
                    var first = it.Current;
                    var second = it.MoveNext()
                        ? it.Current
                        : throw new ArgumentException("source must have an even number of elements");
                    yield return resultSelector(first, second);
                }
            }
        }
    }
}
