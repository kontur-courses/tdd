using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public Point Center { get; }
        const int PointCount = 120;

        public Spiral(Point center)
        {
            Center = center;
        }

        public IEnumerable<Point> GetSpiralPoints()
        {
            /// x = rcos(phi)
            /// y = rsin(phi)
            for (var i = 0; i < PointCount; i++)
            {
                var x = (int)(i * 1.5 * Math.Cos(i * 0.5)) + Center.X;
                var y = (int)(i * 1.5 * Math.Sin(i * 0.5)) + Center.Y;
                yield return new Point(x, y);
            }
        }
    }
}
