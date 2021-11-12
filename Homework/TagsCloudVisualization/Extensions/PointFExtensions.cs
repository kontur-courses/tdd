using System;
using System.Drawing;
using System.Numerics;

namespace TagsCloudVisualization.Extensions
{
    public static class PointFExtensions
    {
        public static float DistanceTo(this PointF from, PointF to)
        {
            return (float) Math.Sqrt(Math.Pow(Math.Abs(from.X - to.X), 2) + Math.Pow(Math.Abs(from.Y - to.Y), 2));
        }
    }
}