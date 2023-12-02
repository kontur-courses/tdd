using System.Drawing;

namespace TagCloudTests;

public static class Extensions
{
    public static IEnumerable<Tuple<T, T>> CartesianProduct<T>(this IEnumerable<T> source)
    {
        var array = source.ToArray();
        if (array.Length <= 1)
            throw new ArgumentException();
        for (int i = 0; i < array.Length; i++)
            for (int j = i + 1; j < array.Length; j++)
                yield return Tuple.Create(array[i], array[j]);
    }

    public static bool HasIntersectedRectangles(this IEnumerable<Rectangle> rectangles)
    {
        foreach (var (first, second) in rectangles.CartesianProduct())
            if (first.IntersectsWith(second))
                return true;

        return false;
    }
}