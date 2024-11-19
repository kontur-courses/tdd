using System.Drawing;

namespace TagsCloudVisualization.Extensions;

public static class PointExtension
{
    public static Point MoveTo(this Point point, Size direction) =>
        Point.Add(point, direction);

    public static double GetDistanceTo(this Point point1, Point point2) =>
        Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
}

