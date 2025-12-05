using System.Drawing;

namespace TagsCloudVisualization;

public static class PolarMath
{
    public static Point PolarToCartesian(double radius, double angle, Point center)
    {
        var x = center.X + (int)(radius * Math.Cos(angle));
        var y = center.Y + (int)(radius * Math.Sin(angle));
        return new Point(x, y);
    }
}