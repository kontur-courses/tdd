using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Cycle<T>(this IEnumerable<T> enumerable)
        {
            while (true)
                foreach (var element in enumerable)
                    yield return element;
            // ReSharper disable once IteratorNeverReturns
        }
    }
}
