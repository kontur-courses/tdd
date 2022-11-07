using System.Drawing;

namespace TagCloud;

public class CircularCloudLayouter
{
    private readonly Point center;
    public CircularCloudLayouter(Point center)
    {
        this.center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
            throw new ArgumentException("Area of rectangle can't be zero");
        return new Rectangle(center, rectangleSize);
    }
}