using System.Drawing;

namespace TagsCloudVizualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    private readonly Spiral spiral;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new();
        spiral = new(center, 0.02, 0.01);
    }

    public Point CloudCenter => center;
    public IList<Rectangle> Rectangles => rectangles;

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ValidateRectangleSize(rectangleSize);
        
        var currentRectangle = CreateNewRectangle(rectangleSize);
        rectangles.Add(currentRectangle);

        return currentRectangle;
    }

    private void ValidateRectangleSize(Size size)
    {
        if (size.Width <= 0 || size.Height <= 0)
        {
            throw new ArgumentException("Rectangle width and height must be positive");
        }

    }

    private Rectangle CreateNewRectangle(Size rectangleSize)
    {
        Rectangle rectangle;
        do
        {
            var pointOnSpiral = spiral.GetNextPointOnSpiral();
            var rectangleLocation = GetUpperLeftCorner(pointOnSpiral, rectangleSize);
            rectangle = new Rectangle(rectangleLocation, rectangleSize);
        } while (IntersectsExisting(rectangle));

        return rectangle;
    }
    
    private bool IntersectsExisting(Rectangle rectangle)
    {
        return rectangles.Any(rect => rect.IntersectsWith(rectangle));
    }

    private Point GetUpperLeftCorner(Point centerPoint, Size size)
    {
        return new Point(centerPoint.X - size.Width / 2, centerPoint.Y - size.Height / 2);
    }
}