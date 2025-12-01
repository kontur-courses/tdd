using System.Drawing;

namespace TagsCloudLib.Extensions;

public static class RectangleExtensions
{
    public static Point Center(this Rectangle rectangle)
    {
        return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
    }
    
    public static bool Intersects(this Rectangle rectangle, IEnumerable<Rectangle> others)
    {
        return others.Any(r => r.IntersectsWith(rectangle));
    }
}