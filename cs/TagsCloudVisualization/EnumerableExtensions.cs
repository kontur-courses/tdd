using System;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T1, T2>> CartesianProduct<T1, T2>(this IEnumerable<T1> enumerable, IEnumerable<T2> otherEnumerable)
        {
            return enumerable.SelectMany(x => otherEnumerable, Tuple.Create);
        }
    }
}