using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleCollectionExtensions
{
    public static bool CheckForAllPairs(
        this IEnumerable<Rectangle> rectangles,
        Func<(Rectangle, Rectangle), bool> predicate)
    {
        var rectangleList = rectangles.ToList();
        return rectangleList
            .SelectMany((rect1, index) => rectangleList
                .GetRange(index, rectangleList.Count - index)
                .Select(rect2 => (rect1, rect2)))
            .Where(tuple => tuple.Item1 != tuple.Item2)
            .All(predicate);
    }
}