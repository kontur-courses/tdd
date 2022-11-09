using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private readonly Point center;
    
    public CircularCloudLayouter(Point center)
    {
        if (center.IsEmpty || center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("center point is invalid", nameof(center));
        this.center = center;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        throw new NotImplementedException();
    }
}