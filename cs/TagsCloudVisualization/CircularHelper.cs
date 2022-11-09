using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularHelper
{
    public static IEnumerable<Point> EnumeratePointsInArchimedesSpiral(
        float k,
        float angleStep,
        Point center)
    {
        var current = new PointF(center.X, center.Y);
        var angle = 0f;

        while (true)
        {
            yield return Point.Round(current);

            var p = k * angle;
            var x = center.X + p * MathF.Cos(angle);
            var y = center.Y + p * MathF.Sin(angle);

            current = new(x, y);
            angle += angleStep;
        }
    }
}