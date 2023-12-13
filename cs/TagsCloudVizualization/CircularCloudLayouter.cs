using System.Drawing;
using TagsCloudVizualization.Interfaces;
using TagsCloudVizualization.Utility;

namespace TagsCloudVizualization;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    private readonly INextPointProvider pointProvider;

    public CircularCloudLayouter(Point center, INextPointProvider pointProvider)
    {
        this.center = center;
        rectangles = new();
        this.pointProvider = pointProvider;
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
            var nextPoint = pointProvider.GetNextPoint();
            var rectangleLocation = GetUpperLeftCorner(nextPoint, rectangleSize);
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