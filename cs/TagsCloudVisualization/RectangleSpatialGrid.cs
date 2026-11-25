using System.Drawing;

namespace TagsCloudVisualization;

internal class RectangleSpatialGrid
{
    private const int CellSize = 64;

    private readonly Dictionary<(int X, int Y), List<Rectangle>> cells = new();

    public void Add(Rectangle rectangle)
    {
        foreach (var cell in GetCells(rectangle))
        {
            if (!cells.TryGetValue(cell, out var bucket))
            {
                bucket = new List<Rectangle>();
                cells[cell] = bucket;
            }

            bucket.Add(rectangle);
        }
    }

    public bool IntersectsWithAny(Rectangle rectangle)
    {
        var checkedRectangles = new HashSet<Rectangle>();

        foreach (var cell in GetCells(rectangle))
        {
            if (!cells.TryGetValue(cell, out var bucket))
                continue;

            foreach (var existing in bucket)
            {
                if (!checkedRectangles.Add(existing))
                    continue;

                if (existing.IntersectsWith(rectangle))
                    return true;
            }
        }

        return false;
    }

    private static IEnumerable<(int X, int Y)> GetCells(Rectangle rectangle)
    {
        var left = GetCellIndex(rectangle.Left);
        var top = GetCellIndex(rectangle.Top);
        var right = GetCellIndex(rectangle.Right - 1);
        var bottom = GetCellIndex(rectangle.Bottom - 1);

        for (var x = left; x <= right; x++)
        {
            for (var y = top; y <= bottom; y++)
            {
                yield return (x, y);
            }
        }
    }

    private static int GetCellIndex(int coordinate)
    {
        return (int)Math.Floor((double)coordinate / CellSize);
    }
}