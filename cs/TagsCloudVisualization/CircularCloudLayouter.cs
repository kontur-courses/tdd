using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private readonly List<Rectangle> rectanglesInLayout;
    private readonly Spiral spiral;
    private readonly IEnumerator<Point> pointsOnSpiral;

    public Point Center { get; }

    public IEnumerable<Rectangle> Rectangles => rectanglesInLayout;
    
    public CircularCloudLayouter(Point center)
    {
        Center = center;
        rectanglesInLayout = new();
        spiral = new(center, 0.05, 0.01);
        pointsOnSpiral = spiral.GetPointsOnSpiral().GetEnumerator();
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle width and height must be positive");
        
        var currentRectangle = CreateNewRectangle(rectangleSize);
        rectanglesInLayout.Add(currentRectangle);
        return currentRectangle;
    }

    private Rectangle CreateNewRectangle(Size rectangleSize)
    {
        Rectangle rectangle;
        do
        {
            pointsOnSpiral.MoveNext();
            var rectangleLocation = GetUpLeftCorner(pointsOnSpiral.Current, rectangleSize);
            rectangle = new Rectangle(rectangleLocation, rectangleSize);
        }
        while (rectanglesInLayout.Any(rect => rect.IntersectsWith(rectangle)));

        return rectangle;
    }

    private Point GetUpLeftCorner(Point rectangleCenter, Size rectangleSize)
    {
        return rectangleCenter - rectangleSize / 2;
    }
}