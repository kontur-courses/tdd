using System;
using System.Drawing;

namespace CloudLayouter
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point original, Point point)
            => Math.Sqrt(Math.Pow(point.X - original.X, 2) + Math.Pow(point.Y - original.Y, 2));
    }
}