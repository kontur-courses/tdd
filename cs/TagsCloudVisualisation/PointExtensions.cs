using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualisation
{
    public static class PointExtensions
    {
        public static double GetDistanceTo(this Point p1, Point p2)
        {
            var dX = p1.X - p2.X;
            var dY = p1.Y - p2.Y;
            return Math.Sqrt(dX * dX + dY * dY);
        }
        public static Point Displace(this Point p1, int dX, int dY)
        {
            return new Point(p1.X + dX, p1.Y + dY);
        }
    }
}
