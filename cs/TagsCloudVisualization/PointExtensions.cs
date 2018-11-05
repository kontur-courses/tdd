using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point first, PointF second) =>
            Math.Sqrt((first.X - second.X) * (first.X - second.X) + (first.Y - second.Y) * (first.Y - second.Y));
    }
}