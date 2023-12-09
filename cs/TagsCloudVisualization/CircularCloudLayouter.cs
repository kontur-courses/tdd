using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    public List<Rectangle> PlacedRectangles { get; } = new();
    private readonly Point center;
    private int radius;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var coordinate in GetPoints())
        {
            var target = new Rectangle(coordinate, rectangleSize);

            if (!IntersectWithPlaced(target))
            {
                PlacedRectangles.Add(target);

                return target;
            }
        }

        return default;
    }

    private IEnumerable<Point> GetPoints()
    {
        var rnd = new Random();
        while (true)
        {
            // Угол рандомизирую для менее детерминированного распределения, i - счётчик для поворота только на 360
            var angle = rnd.Next(360);
            for (var i = 0; i < 360; angle++, i++)
                yield return PointMath.PolarToCartesian(radius, angle, center);
            radius++;
        }
    }

    private bool IntersectWithPlaced(Rectangle target)
    {
        return PlacedRectangles.Any(target.IntersectsWith);
    }
}