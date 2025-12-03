using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter(Point center) : ICircularCloudLayouter
{
    private readonly List<Rectangle> rectangles = [];
    private readonly SpiralPointGenerator pointGenerator = new(center);


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        while (true)
        {
            var point = pointGenerator.GenerateNextPoint();
            var rect = CreateRectangleWithCenter(point, rectangleSize);

            if (rectangles.Any(r => r.IntersectsWith(rect)))
                continue;
                
            rectangles.Add(rect);
            return rect;
        }
    }

    private Rectangle CreateRectangleWithCenter(Point center, Size size)
    {
        var x = center.X - size.Width / 2;
        var y = center.Y - size.Height / 2;
        return new Rectangle(new Point(x, y), size);
    }
}