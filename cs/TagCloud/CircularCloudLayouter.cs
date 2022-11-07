using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> rectangles = new();
    private readonly IEnumerator<Point> spiralEnumerator;

    public CircularCloudLayouter(Point center)
    {
        spiralEnumerator = GetSpiralPoints(center, 0.01, 0.01).GetEnumerator();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
            throw new ArgumentException("Area of rectangle can't be zero");

        var rectangle = CreateNewRectangle(rectangleSize);
        while (rectangles.Any(r => r.IntersectsWith(rectangle)))
            rectangle = CreateNewRectangle(rectangleSize);

        rectangles.Add(rectangle);

        return rectangle;
    }

    private Rectangle CreateNewRectangle(Size size)
    {
        spiralEnumerator.MoveNext();
        var point = new Point(spiralEnumerator.Current.X - size.Width / 2,
            spiralEnumerator.Current.Y - size.Height / 2);
        return new Rectangle(point, size);
    }

    private static IEnumerable<Point> GetSpiralPoints(Point center, double dR, double dPhi)
    {
        double r = 0;
        double phi = 0;
        while (true)
        {
            r += dR;
            phi += dPhi;
            var x = (int)(r * Math.Cos(phi)) + center.X;
            var y = (int)(r * Math.Sin(phi)) + center.Y;
            yield return new Point(x, y);
        }
    }
}