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
        var location = new Point(
            center.X - rectangleSize.Width / 2,
            center.Y - rectangleSize.Height / 2);

        return new Rectangle(location, rectangleSize);
    }
}
