using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Donatello.Extensions
{
    static class LinqExtensions
    {
        public static IEnumerable<T> Prepend<T>(this T elementToPrepend, IEnumerable<T> source)
        {
            yield return elementToPrepend;
            foreach (var element in source)
            {
                yield return element;
            }
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> source, T elementToAppend)
        {
            foreach (var element in source)
            {
                yield return element;
            }
            yield return elementToAppend;
        }
    }
}
