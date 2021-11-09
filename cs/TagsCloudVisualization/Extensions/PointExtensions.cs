using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point point, Point other) =>
            Math.Sqrt(Math.Pow(point.X - other.Y, 2) + Math.Pow(point.Y - other.Y, 2));
    }
}