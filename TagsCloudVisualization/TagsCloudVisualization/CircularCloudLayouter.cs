using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    private readonly List<Rectangle> rectangles;
    public IEnumerable<Rectangle> Rectangles => rectangles;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
        rectangles = new List<Rectangle>();
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        var rectPosition = new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);

        while (!IsPlaceSuitableForRectangle(new Rectangle(rectPosition, rectangleSize)))
        {
            rectPosition.Y += 1;
        }
        var rectangle = new Rectangle(rectPosition, rectangleSize);
        rectangles.Add(rectangle);
        return rectangle;
    }

    private bool IsPlaceSuitableForRectangle(Rectangle rectangle)
    {
        return rectangles.All(x => !x.IntersectsWith(rectangle));
    }
}