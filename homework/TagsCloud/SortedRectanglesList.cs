using System.Drawing;

namespace TagsCloud;

/// <summary>
/// Вспомогательная структура для хранения прямоугольников, отсортированных по координатам сторон,
/// с возможностью получения прямоугольника по индексу в отсортированном списке.
/// </summary>
public class SortedRectanglesList
{
    private readonly Dictionary<Direction, SortedList<int, Rectangle>> _sortedRectangles;

    public SortedRectanglesList()
    {
        var noEqualityComparer = new NoEqualityComparer<int>();

        _sortedRectangles = new Dictionary<Direction, SortedList<int, Rectangle>>(4)
        {
            { Direction.Left, new(noEqualityComparer) },
            { Direction.Right, new(noEqualityComparer) },
            { Direction.Up, new(noEqualityComparer) },
            { Direction.Down, new(noEqualityComparer) },
        };
    }

    public int Count { get; private set; }

    public void Add(Rectangle rectangle)
    {
        _sortedRectangles[Direction.Left].Add(-rectangle.Left, rectangle);
        _sortedRectangles[Direction.Right].Add(rectangle.Right, rectangle);
        _sortedRectangles[Direction.Up].Add(-rectangle.Top, rectangle);
        _sortedRectangles[Direction.Down].Add(rectangle.Bottom, rectangle);

        Count++;
    }

    public Rectangle Get(Direction sortingDirection, int index)
    {
        if (!_sortedRectangles.TryGetValue(sortingDirection, out var rectangles))
        {
            throw new ArgumentException($"Unsupported sorting direction: {sortingDirection}.");
        }

        if (index < 0 || index >= Count)
        {
            throw new IndexOutOfRangeException($"Index was out of range: {index}.");
        }

        return rectangles.Values[index];
    }

    public bool HasIntersection(
        Rectangle rectangle,
        Direction sortingDirection,
        int startIndex,
        out int intersectedRectangleIndex)
    {
        if (!_sortedRectangles.TryGetValue(sortingDirection, out var rectangles))
        {
            throw new ArgumentException($"Unsupported sorting direction: {sortingDirection}.");
        }

        if (startIndex < 0)
        {
            throw new IndexOutOfRangeException($"Index was out of range: {startIndex}.");
        }

        for (var i = startIndex; i < rectangles.Count; i++)
        {
            if (rectangle.IntersectsWith(rectangles.Values[i]))
            {
                intersectedRectangleIndex = i;
                return true;
            }
        }

        intersectedRectangleIndex = -1;
        return false;
    }
}
