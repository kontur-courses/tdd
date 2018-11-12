using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointFExtensions
    {
        public static double DistanceTo(this PointF p1, PointF p2)
        {
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }
}
