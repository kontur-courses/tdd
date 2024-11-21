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
        public static int CalculateDistanceBetween(this Point current, Point other)
        {
            return (int)Math.Ceiling(Math.Sqrt((current.X - other.X) * (current.X - other.X) + (current.Y - other.Y) * (current.Y - other.Y)));
        }
    }
}
