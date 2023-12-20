using Aspose.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> _rectangles = new();

    public CircularCloudLayouter(Point center)
    {
        Center = center;
    }

    public Point Center { get; }
    public IReadOnlyCollection<Rectangle> GetRectangles => _rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Width and height of rectangle must be positive");

        var offsetBeforeCenter = rectangleSize.GetGreaterHalf();

        foreach (var coord in Layout.GetNextCoord(Center))
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