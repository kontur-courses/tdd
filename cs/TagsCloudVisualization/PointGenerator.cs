using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class PointGenerator : IEnumerable<Point>
    {
        private double spiralAngle { get; set; }
        private Point center { get; set; }
        private double delta { get; set; }

        public PointGenerator(double spiralAngle, Point center, double delta)
        {
            this.spiralAngle = spiralAngle;
            this.center = center;
            this.delta = delta;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            while (true)
            {
                yield return
                      new Point(center.X + (int)(spiralAngle * Math.Cos(spiralAngle)),
                          center.Y + (int)(spiralAngle * Math.Sin(spiralAngle)));
                spiralAngle += delta;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }
    }
}
