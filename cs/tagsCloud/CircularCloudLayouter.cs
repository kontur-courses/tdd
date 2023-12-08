using System.Drawing;

namespace TagsCloud;

public class CircularCloudLayouter : ICircularCloudLayouter
{
    public List<Rectangle> Rectangles { get; }

    private readonly ISpiral spiral;

    public CircularCloudLayouter(ISpiral spiral)
    {
        this.spiral = spiral;
        Rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            throw new ArgumentException("Sides of the rectangle should not be non-positive");
        var rect = CreateNextRectangle(rectangleSize);
        Rectangles.Add(rect);
        return rect;
    }

    private Rectangle CreateNextRectangle(Size rectangleSize)
    {
        var point = spiral.GetPoint();
        var rect = new Rectangle(point, rectangleSize);
        while (!HasNoIntersections(rect))
        {
            point = spiral.GetPoint();
            rect = new Rectangle(point, rectangleSize);
        }

        return rect;
    }

    private bool HasNoIntersections(Rectangle rect)
    {
        for (var i = Rectangles.Count - 1; i >= 0; i--)
            if (Rectangles[i].IntersectsWith(rect))
                return false;
        return true;
    }
}