using System.Drawing;
using TagsCloudVizualization.Interfaces;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    private readonly Spiral spiral;
    private readonly IEnumerator<Point> spiralPointsEnumerator;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new();
        spiral = new(center, 0.02, 0.01);
        spiralPointsEnumerator = spiral.GetPointsOnSpiral().GetEnumerator();
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
    
    private void ValidateRectangleSize(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
        {
            throw new ArgumentException("Width and height of the rectangle must be greater than zero");
        }
    }

    private Rectangle CreateNewRectangle(Size rectangleSize)
    {
        while (true)
        {
            spiralPointsEnumerator.MoveNext();
            var rectangleLocation = GetUpperLeftCorner(spiralPointsEnumerator.Current, rectangleSize);
            var rectangle = new Rectangle(rectangleLocation, rectangleSize);

            if (!RectanglesIntersect(rectangle))
            {
                return rectangle;
            }
        }
    }

    private Point GetUpperLeftCorner(Point rectangleCenter, Size rectangleSize)
    {
        return new Point(rectangleCenter.X - rectangleSize.Width / 2, rectangleCenter.Y - rectangleSize.Height / 2);
    }

    private bool RectanglesIntersect(Rectangle newRectangle)
    {
        return rectangles.Any(rect => rect.IntersectsWith(newRectangle));
    }
}