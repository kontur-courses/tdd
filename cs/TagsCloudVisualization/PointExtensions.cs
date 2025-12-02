using System.Drawing;

namespace TagsCloudVisualization;

public static class PointExtensions
{
    public static double GetDistance(this Point firstPoint, Point secondPoint)
    {
        return Math.Sqrt(Math.Pow(firstPoint.X - secondPoint.X, 2) + Math.Pow(firstPoint.Y - secondPoint.Y, 2));
    }
}