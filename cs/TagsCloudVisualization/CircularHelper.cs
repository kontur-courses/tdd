using System.Drawing;

namespace TagsCloudVisualization;

public static class CircularHelper
{
    public static IEnumerable<Point> EnumeratePointsInArchimedesSpiral(float k,
        float angleStep,
        Point center, float startAngle = 0f)
    {
        var current = new PointF(center.X, center.Y);
        var angle = startAngle;

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