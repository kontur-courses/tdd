using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        public static double DistanceTo(this Point point, Point anotherPoint)
        {
            var squaredX = (point.X - anotherPoint.X) * (point.X - anotherPoint.X);
            var squaredY = (point.Y - anotherPoint.Y) * (point.Y - anotherPoint.Y);
            return Math.Sqrt(squaredX + squaredY);
        }
    }
}
