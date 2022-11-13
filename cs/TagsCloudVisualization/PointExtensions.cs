using System.Drawing;

namespace TagsCloudVisualization;

public static class PointExtensions
{
    public static double DistanceTo(this Point source, Point destination)
    {
        return Math.Sqrt(Math.Pow(destination.X - source.X, 2) + Math.Pow(destination.Y - source.Y, 2));
    }
}