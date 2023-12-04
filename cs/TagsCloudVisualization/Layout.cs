using System.Drawing;

namespace TagsCloudVisualization;

public class Layout
{
    private readonly HashSet<Point> occupiedPixels = new();

    public Layout(Point center)
    {
        Center = center;
    }
    
    public Point Center { get; }
    
    public IEnumerable<Point> GetNextCoord()
    {
        yield return Center + new Size(0, 0);
        var offset = 1;

        while (true)
        {
            for (var dx = -offset; dx <= offset; dx++)
            for (var dy = -offset; dy <= offset; dy++)
            {
                if (!(Math.Abs(dx) != offset || Math.Abs(dy) != offset))
                    continue;

                yield return Center + new Size(dx, dy);
            }

            offset++;
        }
    }

    public bool CanPutInCoords(Rectangle rectangle)
    {
        for (var i = rectangle.Left; i <= rectangle.Right; i++)
        for (var j = rectangle.Top; j <= rectangle.Bottom; j++)
            if (occupiedPixels.Contains(new Point(i, j)))
                return false;

        return true;
    }

    public void OccupyCoords(Rectangle rectangle)
    {
        for (var i = rectangle.Left; i <= rectangle.Right; i++)
        for (var j = rectangle.Top; j <= rectangle.Bottom; j++)
            occupiedPixels.Add(new Point(i, j));
    }
}