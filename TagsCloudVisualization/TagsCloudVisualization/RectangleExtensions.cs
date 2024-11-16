using System.Drawing;

namespace TagsCloudVisualization;

public static class RectangleExtensions
{
    public static bool ContainsFloat(this Rectangle rectangle, PointF point)
    {
        return rectangle.X <= point.X
               && point.X < rectangle.Right
               && rectangle.Y <= point.Y
               && point.Y < rectangle.Bottom;
    }

    public static bool DistanceToOtherIsNotGreaterThan(this Rectangle rect, Rectangle other, int distance)
    {
        var center1 = RectangleCenter(rect);
        var center2 = RectangleCenter(other);
        var actualDistance = center1.SquaredDistanceTo(center2);
        return actualDistance <= distance;
    }

    public static Point RectangleCenter(this Rectangle rectangle)
    {
        return new Point((rectangle.X + rectangle.Right) / 2, (rectangle.Y + rectangle.Bottom) / 2);
    }
}