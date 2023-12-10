using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center)
{
    private int radius;
    private int minDimension = int.MaxValue;
    private readonly List<Rectangle> placedRectangles = new();

    public IReadOnlyList<Rectangle> PlacedRectangles => placedRectangles.AsReadOnly();

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        minDimension = Math.Min(minDimension, rectangleSize.Height);

        foreach (var coordinate in GetPoints())
        {
            var target = new Rectangle(
                new Point(
                    coordinate.X - rectangleSize.Width / 2,
                    coordinate.Y - rectangleSize.Height / 2
                ), rectangleSize);

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
        var movableX = true;
        var movableY = true;
        while ((movableX || movableY) && !IntersectWithPlaced(target))
        {
            if (movableX)
            {
                target.X += Math.Sign(center.X - target.X);
                if (center.X == target.X)
                    movableX = false;

                if (IntersectWithPlaced(target))
                {
                    target.X -= Math.Sign(center.X - target.X);
                    movableX = false;
                }
            }

            if (movableY)
            {
                target.Y += Math.Sign(center.Y - target.Y);
                if (center.Y == target.Y)
                    movableY = false;

                if (IntersectWithPlaced(target))
                {
                    target.Y -= Math.Sign(center.Y - target.Y);
                    movableY = false;
                }
            }
        }
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