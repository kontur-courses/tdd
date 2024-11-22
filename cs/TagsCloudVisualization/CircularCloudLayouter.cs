using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    private readonly Spiral spiral;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
        spiral = new Spiral(center);
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        Rectangle rectangle;
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentException($"Rectangle size ({rectangleSize}) should be positive");
        }

        do
        {
            Point point = spiral.GetNextPointOnSpiral();
            point.Offset(-rectangleSize.Width / 2, -rectangleSize.Height / 2);
            rectangle = new Rectangle(point, rectangleSize);
        } while (rectangles.Any(r => r.IntersectsWith(rectangle)));

        rectangles.Add(rectangle);
        return rectangle;
    }

    public Size getCloudSize()
    {
        var left = rectangles.Min(x => x.Left);
        var right = rectangles.Max(x => x.Right);
        var top = rectangles.Min(x => x.Top);
        var bottom = rectangles.Max(x => x.Bottom);
        var size = new Size( Math.Abs(right) + Math.Abs(left),Math.Abs(bottom) + Math.Abs(top));
        return size;
    }
    public List<Rectangle> GetRectangles()
    {
        return rectangles;
    }
}