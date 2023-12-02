using System.Drawing;

namespace Utility;

public static class PolarMath
{
    public static Point PolarToCartesian(int radius, int angle, Point offset = default)
    {
        return new Point(
            (int)(radius * Math.Cos(angle * Math.PI / 180.0)) + offset.X,
            (int)(radius * Math.Sin(angle * Math.PI / 180.0)) + offset.Y
        );
    }
}