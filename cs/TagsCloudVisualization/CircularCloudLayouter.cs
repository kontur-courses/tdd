using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Layout layout;
    private readonly List<Rectangle> _rectangles = new();

    public CircularCloudLayouter(Point center)
    {
        layout = new Layout(center);
    }

    public Point Center => layout.Center;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Width and height of rectangle must be positive");

        var offsetBeforeCenter = rectangleSize.GetGreaterHalf();

        foreach (var coord in layout.GetNextCoord())
        {
            var now = new Rectangle(coord - offsetBeforeCenter, rectangleSize);
            
            if (!CanPutRectangle(now))
                continue;

            PutRectangle(now);

            return now;
        }

        throw new IndexOutOfRangeException("Cannot find suitable place on the layout");
    }
    
    private bool CanPutRectangle(Rectangle rectangle)
    {
        return !_rectangles.Any(rectangle.IntersectsWith);
    }

    private void PutRectangle(Rectangle rectangle)
    {
        _rectangles.Add(rectangle);
    }
}