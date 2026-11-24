using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point _center;
    private readonly List<Rectangle> _rectangles = new();

    public CircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        foreach (var point in GetSpiralPoints())
        {
            var rectangle = new Rectangle(
                point.X - rectangleSize.Width / 2,
                point.Y - rectangleSize.Height / 2,
                rectangleSize.Width,
                rectangleSize.Height);

            if (_rectangles.Any(r => r.IntersectsWith(rectangle)))
                continue;

            _rectangles.Add(rectangle);

            return rectangle;
        }

        throw new InvalidOperationException();
    }

    private IEnumerable<Point> GetSpiralPoints()
    {
        var radiusFactor = 0.8;
        var angleStep = 0.05;

        for (var angle = 0.0; ; angle += angleStep)
        {
            var radius = radiusFactor * angle;

            var x = _center.X + radius * Math.Cos(angle);
            var y = _center.Y + radius * Math.Sin(angle);

            yield return new Point(
                (int)Math.Round(x),
                (int)Math.Round(y));
        }
    }
}
