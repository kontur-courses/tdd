using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CirclePointGenerator : IEnumerable<Point>
    {
        private Point center { get; set; }
        private double angleStep { get; set; }

        public CirclePointGenerator(Point center, double angleStep = 0.1)
        {
            this.center = center;
            this.angleStep = angleStep;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            var angle = 0.0;
            var radius = 0.0;
            while (true)
            {
                var x = center.X + (int)(radius * Math.Cos(angle));
                var y = center.Y + (int)(radius * Math.Sin(angle));
                angle += angleStep;
                if (angle >= 2*Math.PI)
                {
                    radius += 5;
                    angle = 0;
                }

                yield return new Point(x,y);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
