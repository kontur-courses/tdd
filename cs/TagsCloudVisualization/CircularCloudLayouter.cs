using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;

    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        return new Rectangle(
            center.X - rectangleSize.Width / 2,
            center.Y - rectangleSize.Height / 2,
            rectangleSize.Width,
            rectangleSize.Height);
    }
}