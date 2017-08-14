using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static int DistanceFrom(this Point fromPoint, Point toPoint)
        {
            var dx = (toPoint.X - fromPoint.X);
            var dy = (toPoint.Y - fromPoint.Y);
            return (int)Math.Sqrt(Math.Pow(dx,2)+Math.Pow(dy,2));
        }

    }
}