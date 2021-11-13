using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point source, Point other)
        {
            return Math.Sqrt(Math.Pow(source.X - other.X, 2) + Math.Pow(source.Y - other.Y, 2));
        }
    }
}