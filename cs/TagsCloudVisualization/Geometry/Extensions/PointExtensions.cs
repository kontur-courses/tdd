using System.Drawing;

namespace TagsCloudVisualization.Geometry.Extensions;

public static class PointExtensions
{
    public static double DistanceTo(this Point point, Point other)
    {
        var dx = point.X - other.X;
        var dy = point.Y - other.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    public static bool IsZero(this Point point)
    {
        return Math.Abs(point.X) < double.Epsilon && Math.Abs(point.Y) < double.Epsilon;
    }
}