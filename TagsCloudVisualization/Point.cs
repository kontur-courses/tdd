using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static int DistanceFrom(this Point fromPoint, Point toPoint)
        {
            return (int)Math.Sqrt((toPoint.X-fromPoint.X)^2+(toPoint.Y-fromPoint.Y)^2);
        }

    }
}