using SixLabors.ImageSharp;

namespace TagsCloudVisualization;

public static class PointFExtensions
{
    public static void ConvertToCartesian(this ref PointF point)
    {
        var (radius, angle) = point;

        var x = radius * (float)Math.Cos(angle);
        var y = radius * (float)Math.Sin(angle);

        (point.X, point.Y) = (x, y);
    }

    public static void Center(this ref PointF point, ref PointF center)
    {
        point.X += center.X;
        point.Y += center.Y;
    }

    public static void ApplyOffset(this ref PointF point, float offsetX, float offsetY)
    {
        point.X += offsetX;
        point.Y += offsetY;
    }
}