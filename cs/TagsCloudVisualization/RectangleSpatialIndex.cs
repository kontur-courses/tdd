using System.Drawing;

namespace TagsCloudVisualization;

internal sealed class RectangleSpatialIndex
{
    private readonly List<Rectangle> rectangles = new();
    private readonly Dictionary<(long X, long Y), List<int>> cells = new();
    private int[] queryMarks = Array.Empty<int>();

    private int cellSize;
    private int queryVersion;

    public int Count => rectangles.Count;

    public void Add(Rectangle rectangle)
    {
        EnsureCellSize(rectangle);

        var rectangleIndex = rectangles.Count;
        rectangles.Add(rectangle);

        EnsureQueryMarksCapacity();
        AddToCells(rectangleIndex, rectangle);
    }

    public bool IntersectsAny(Rectangle candidate)
    {
        if (rectangles.Count == 0)
            return false;

        EnsureCellSize(candidate);

        var currentQueryVersion = NextQueryVersion();
        var range = GetCellRange(candidate);

        for (var x = range.Left; x <= range.Right; x++)
        {
            for (var y = range.Top; y <= range.Bottom; y++)
            {
                if (!cells.TryGetValue((x, y), out var rectangleIndices))
                    continue;

                foreach (var rectangleIndex in rectangleIndices)
                {
                    if (queryMarks[rectangleIndex] == currentQueryVersion)
                        continue;

                    queryMarks[rectangleIndex] = currentQueryVersion;

                    if (rectangles[rectangleIndex].IntersectsWith(candidate))
                        return true;
                }
            }
        }

        return false;
    }

    private void EnsureCellSize(Rectangle rectangle)
    {
        var requiredCellSize = Math.Max(rectangle.Width, rectangle.Height);

        if (requiredCellSize <= cellSize)
            return;

        cellSize = requiredCellSize;
        cells.Clear();

        for (var i = 0; i < rectangles.Count; i++)
            AddToCells(i, rectangles[i]);
    }

    private void EnsureQueryMarksCapacity()
    {
        if (queryMarks.Length < rectangles.Count)
            Array.Resize(ref queryMarks, rectangles.Count);
    }

    private int NextQueryVersion()
    {
        if (queryVersion == int.MaxValue)
        {
            Array.Clear(queryMarks, 0, queryMarks.Length);
            queryVersion = 0;
        }

        return ++queryVersion;
    }

    private void AddToCells(int rectangleIndex, Rectangle rectangle)
    {
        var range = GetCellRange(rectangle);

        for (var x = range.Left; x <= range.Right; x++)
        {
            for (var y = range.Top; y <= range.Bottom; y++)
            {
                var cell = (x, y);

                if (!cells.TryGetValue(cell, out var rectangleIndices))
                {
                    rectangleIndices = new List<int>();
                    cells.Add(cell, rectangleIndices);
                }

                rectangleIndices.Add(rectangleIndex);
            }
        }
    }

    private (long Left, long Top, long Right, long Bottom) GetCellRange(Rectangle rectangle)
    {
        var right = (long)rectangle.X + rectangle.Width - 1;
        var bottom = (long)rectangle.Y + rectangle.Height - 1;

        return (
            GetCellCoordinate(rectangle.X),
            GetCellCoordinate(rectangle.Y),
            GetCellCoordinate(right),
            GetCellCoordinate(bottom));
    }

    private long GetCellCoordinate(long coordinate)
    {
        var quotient = coordinate / cellSize;

        return coordinate % cellSize < 0
            ? quotient - 1
            : quotient;
    }
}