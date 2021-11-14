using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal static class PointExtensions
    {
        public static double GetDistanceTo(this Point start, Point finish)
        {
            var xDist = Math.Abs(finish.X - start.X);
            var yDist = Math.Abs(finish.Y - start.Y);
            return Math.Sqrt(xDist * xDist + yDist * yDist);
        }
    }
}
