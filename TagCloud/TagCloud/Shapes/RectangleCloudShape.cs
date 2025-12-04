using System.Drawing;

namespace TagCloud.Shapes;
public class RectangleCloudShape(Rectangle rectangle) : ICloudShape
{
    private Rectangle Rectangle { get; } = rectangle;
    
    public Rectangle BoundingBox => Rectangle;

    public Point Center => new(
        Rectangle.Left + Rectangle.Width / 2,
        Rectangle.Top + Rectangle.Height / 2);

    public bool IntersectsWith(ICloudShape otherShape)
    {
        if (otherShape is RectangleCloudShape rectangle)
            return Rectangle.IntersectsWith(rectangle.Rectangle);

        throw new NotSupportedException("Processing of such shapes has not yet been implemented");
    }

    public ICloudShape Shift(int dx, int dy)
    {
        var shifted = Rectangle with { X = Rectangle.Left + dx, Y = Rectangle.Top + dy };
        return new RectangleCloudShape(shifted);
    }
}