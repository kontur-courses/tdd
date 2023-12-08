using System.Drawing;

namespace TagsCloudVisualization;

public static class MathWithPoints
{
    public static Point PolarToCartesian(int radius, int angle, Point offset = default)
    {
        return new Point(
            (int)(radius * Math.Cos(angle * Math.PI / 180.0)) + offset.X,
            (int)(radius * Math.Sin(angle * Math.PI / 180.0)) + offset.Y
        );
    }

    public static double DistanceToCenter(Point coordinate, Point center = default)
    {
        return Math.Sqrt(
            (coordinate.X - center.X) * (coordinate.X - center.X) +
            (coordinate.Y - center.Y) * (coordinate.Y - center.Y)
        );
    }
}