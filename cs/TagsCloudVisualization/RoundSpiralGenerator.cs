using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class RoundSpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly double k;

        public RoundSpiralGenerator(Point center, double k)
        {
            this.center = center;
            this.k = k;
        }

        private Point PolarToCartesian(double r, double phi)
        {
            var x = r * Math.Cos(phi) + center.X;
            var y = r * Math.Sin(phi) + center.Y;
            return new Point((int)x, (int)y);
        }
        public IEnumerator<Point> GetEnumerator()
        {
            var phi = 0d;
            int step = 16;
            var dPhi = Math.PI / step;
            //r = k*ф;
            var r = 0d;
            yield return PolarToCartesian(r, phi);
            while (true)
            {
                phi += dPhi;
                r = k * phi;
                yield return PolarToCartesian(r, phi);
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}