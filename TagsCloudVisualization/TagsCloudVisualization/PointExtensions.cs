using System.Drawing;

namespace TagsCloudVisualization;

public static class PointExtensions
{
    public static Point Subtract(this Point point, Point other)
    {
        return new Point(point.X - other.X, point.Y - other.Y);
    }
    
    public static Point Add(this Point point, Point other)
    {
        return new Point(point.X + other.X, point.Y + other.Y);
    }
}