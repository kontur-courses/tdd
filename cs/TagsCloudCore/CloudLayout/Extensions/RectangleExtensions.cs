using System.Drawing;

namespace TagsCloudCore.CloudLayout.Extensions;

public static class RectangleExtensions
{
    public static bool IntersectsAny(this Rectangle candidate, IEnumerable<Rectangle> rectangles)
    {
        if (rectangles == null)
        {
            throw new ArgumentNullException(nameof(rectangles), "Existing rectangles can't be null");
        }
        
        return rectangles.Any(r => r.IntersectsWith(candidate));
    }
    
    public static Point Center(this Rectangle rectangle)
    {
        return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
    }
    
    public static double GetDistanceToPoint(this Rectangle rectangle, Point point)
    {
        var cx = rectangle.X + rectangle.Width / 2;
        var cy = rectangle.Y + rectangle.Height / 2;

        var dx = cx - point.X;
        var dy = cy - point.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }
}