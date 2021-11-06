using System;
using System.Collections.Generic;

namespace Heds.Utilities
{
    public static class EnumerableExtensions
    {
        public static Dictionary<TIn, TValue> ToDictionary<TValue, TIn>(
            this IEnumerable<TIn> source,
            Func<TIn, int, TValue> valueSelector)
        {
            var result = new Dictionary<TIn, TValue>();

            var i = 0;
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    result[current] = valueSelector(current, i);
                    i++;
                }
            }
            
            return result;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}