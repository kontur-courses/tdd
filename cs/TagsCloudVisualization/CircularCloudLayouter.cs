using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private readonly List<Rectangle> placedRectangles = new();
    public List<Rectangle> PlacedRectangles => new(placedRectangles);
    private int radius;
    private int minDimension = int.MaxValue;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        minDimension = Math.Min(minDimension, Math.Min(rectangleSize.Height, rectangleSize.Width));

        foreach (var coordinate in GetPoints())
        {
            var target = new Rectangle(coordinate, rectangleSize);

            if (!IntersectWithPlaced(target))
            {
                CompactRectangle(ref target);

                placedRectangles.Add(target);
                
                return target;
            }
        }

        return default;
    }

    private void CompactRectangle(ref Rectangle target)
    {
        while (center.Y != target.Y && !IntersectWithPlaced(target))
        {
            target.Y += Math.Sign(center.Y - target.Y);
        }
        if (center.Y != target.Y)
            target.Y -= Math.Sign(center.Y - target.Y);
        
        while (center.X != target.X && !IntersectWithPlaced(target))
        {
            target.X += Math.Sign(center.X - target.X);
        }
        if (center.X != target.X)
            target.X -= Math.Sign(center.X - target.X);
    }

    private IEnumerable<Point> GetPoints()
    {
        var rnd = new Random();
        while (true)
        {
            var angle = rnd.Next(360);
            for (var i = 0; i < 360; angle++, i++)
                yield return PointMath.PolarToCartesian(radius, angle, center);
            radius += minDimension;
        }
    }

    private bool IntersectWithPlaced(Rectangle target)
    {
        return placedRectangles.Any(target.IntersectsWith);
    }
}