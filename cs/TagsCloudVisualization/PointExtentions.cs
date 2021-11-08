using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtentions
    {
        public static double GetDistance(this Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X)
                + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}
