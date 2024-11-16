using System.Drawing;

namespace TagsCloudVisualization;

public static class PointExtensions
{
    public static int SquaredDistanceTo(this Point p1, Point p2)
    {
        return (int)(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
    }
}