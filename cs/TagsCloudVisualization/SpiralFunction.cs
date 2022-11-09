using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class SpiralFunction
    {
        private const double Dr = 0.01; // delta radius
        private const double Fi = 0.0368; // angle

        public static Func<int, Point> GetPointFinderFunction(Point Center)
        {
            return (int arg) => new Point(
                (int)(Center.X + Dr * arg * Math.Cos(Fi * arg)),
                (int)(Center.Y + Dr * arg * Math.Sin(Fi * arg)));
        }
    }
}
