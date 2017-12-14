using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Donatello.StandardLibrary
{
    public static class EnumerableFunctions
    {
        public static IEnumerable<TResult> map<TSource, TResult>(
            Func<TSource, TResult> selector,
            IEnumerable<TSource> source)
            => source.Select(selector);

        public static IEnumerable<TResult> map<TSource1, TSource2, TResult>(
            Func<TSource1, TSource2, TResult> selector,
            IEnumerable<TSource1> source1,
            IEnumerable<TSource2> source2)
            => source1.Zip(source2, selector);

        public static IEnumerable<TResult> map<TSource1, TSource2, TSource3, TResult>(
            Func<TSource1, TSource2, TSource3, TResult> selector,
            IEnumerable<TSource1> source1,
            IEnumerable<TSource2> source2,
            IEnumerable<TSource3> source3)
        {
            if (source1 == null) throw new ArgumentNullException(nameof(source1));
            if (source2 == null) throw new ArgumentNullException(nameof(source2));
            if (source3 == null) throw new ArgumentNullException(nameof(source3));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            using (var e1 = source1.GetEnumerator())
            using (var e2 = source2.GetEnumerator())
            using (var e3 = source3.GetEnumerator())
            {
                while (e1.MoveNext() && e2.MoveNext() && e3.MoveNext())
                    yield return selector(e1.Current, e2.Current, e3.Current);
            }
        }

        public static IEnumerable<TResult> Flatmap<TSource, TResult>(
            Func<TSource, IEnumerable<TResult>> selector,
            IEnumerable<TSource> source)
            => source.SelectMany(selector);

        public static IEnumerable<TSource> filter<TSource>(
            Func<TSource, bool> predicate,
            IEnumerable<TSource> source)
            => source.Where(predicate);

        public static TSource reduce<TSource>(
            Func<TSource, TSource, TSource> func,
            IEnumerable<TSource> source)
            => source.Aggregate(func);

        public static TAccumulate reduce<TSource, TAccumulate>(
            Func<TAccumulate, TSource, TAccumulate> func,
            TAccumulate seed,
            IEnumerable<TSource> source)
            => source.Aggregate(seed, func);

        public static IEnumerable<TResult> mapIndexed<TSource, TResult>(
            Func<TSource, int, TResult> selector,
            IEnumerable<TSource> source)
            => source.Select(selector);

        public static IEnumerable<TSource> filterIndexed<TSource>(
            Func<TSource, int, bool> predicate,
            IEnumerable<TSource> source)
            => source.Where(predicate);

        public static IEnumerable<T> flatten<T>(
            IEnumerable<IEnumerable<T>> source)
            => source.SelectMany(t => t);

        public static IEnumerable<T> distinct<T>(
            IEnumerable<T> source)
            => source.Distinct();

        public static IEnumerable<TSource> distinctBy<TSource, TProperty>(
            Func<TSource, TProperty> propertySelector,
            IEnumerable<TSource> source)
            => source.GroupBy(propertySelector).Select(element => element.First());

        public static IEnumerable<IGrouping<TProperty, TSource>> groupBy<TSource, TProperty>(
            Func<TSource, TProperty> propertySelector,
            IEnumerable<TSource> source)
            => source.GroupBy(propertySelector);

        public static IEnumerable<TSource> concat<TSource>(
            params IEnumerable<TSource>[] sources)
            => flatten(sources);

        public static IEnumerable<TSource> rest<TSource>(
            IEnumerable<TSource> source)
            => source.Skip(1);

        //TODO: Option
        public static TSource first<TSource>(
            IEnumerable<TSource> source)
            => source.FirstOrDefault();

        public static IEnumerable<TSource> first<TSource>(
            int count,
            IEnumerable<TSource> source)
            => source.Take(count);

        //TODO: Option
        public static TSource first<TSource>(
            Func<TSource, bool> predicate,
            IEnumerable<TSource> source)
            => source.FirstOrDefault(predicate);

        public static IEnumerable<TSource> first<TSource>(
            int count,
            Func<TSource, bool> predicate,
            IEnumerable<TSource> source)
            => source.Where(predicate).Take(count);

        //TODO: Option
        public static TSource last<TSource>(
            IEnumerable<TSource> source)
            => source.LastOrDefault();

        public static IEnumerable<TSource> last<TSource>(
            int count,
            IEnumerable<TSource> source)
            => last(count, (_) => true, source);

        //TODO: Option
        public static TSource last<TSource>(
            Func<TSource, bool> predicate,
            IEnumerable<TSource> source)
            => source.LastOrDefault(predicate);

        public static IEnumerable<TSource> last<TSource>(
            int count,
            Func<TSource, bool> predicate,
            IEnumerable<TSource> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "must be 0 or greater");

            var buffer = new LinkedList<TSource>();

            foreach (var value in source)
            {
                if (predicate(value))
                {
                    buffer.AddLast(value);
                    if (buffer.Count > count)
                        buffer.RemoveFirst();
                }
            }

            return buffer;
        }

        public static IEnumerable<TSource> skip<TSource>(
            int count,
            IEnumerable<TSource> source)
            => source.Skip(count);
    }
}
