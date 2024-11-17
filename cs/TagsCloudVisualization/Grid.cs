using System.Drawing;

namespace TagsCloudVisualization;

public class Grid
{
    private readonly int cellSize;
    private readonly Dictionary<Point, List<Rectangle>> cells = new();

    public Grid(int cellSize = 50)
    {
        this.cellSize = cellSize;
    }
    public void AddRectangle(Rectangle rectangle)
    {
        var cellsToUpdate = GetCellsCoveredByRectangle(rectangle);
        
        foreach (var cell in cellsToUpdate)
        {
            if (!cells.ContainsKey(cell))
            {
                cells[cell] = new List<Rectangle>();
            }

            cells[cell].Add(rectangle);
        }
    }

    public bool IsIntersecting(Rectangle rectangle)
    {
        var cellsToCheck = GetCellsCoveredByRectangle(rectangle);
        
        foreach (var cell in cellsToCheck)
        {
            if (!cells.TryGetValue(cell, out var rectanglesInCell))
            {
                continue;
            }

            if (rectanglesInCell.Any(r => r.IntersectsWith(rectangle)))
            {
                return true;
            }
        }
        
        return false;
    }

    private IEnumerable<Point> GetCellsCoveredByRectangle(Rectangle rectangle)
    {
        var startX = rectangle.Left / cellSize;
        var endX = rectangle.Right / cellSize;
        var startY = rectangle.Top / cellSize;
        var endY = rectangle.Bottom / cellSize;

        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                yield return new Point(x, y);
            }
        }
    }
}