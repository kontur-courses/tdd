using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class ArchimedeanSpiral
    {
        private readonly Random random = new Random(42);

        public ArchimedeanSpiral(Point center, double delta = 0.1, double parameter = 1.0)
        {
            this.parameter = parameter;
            this.delta = delta;
            angle = center.X * center.X + center.Y * center.Y;
        }

        private readonly double delta;
        private readonly double parameter;
        private double angle;

        public IEnumerable<Point> GetNewPointLazy()
        {
            while (true)
            {
                angle += delta;
                var result = new Point(
                    (int) Math.Round(parameter * angle * Math.Sin(angle)) + random.Next(2, 10),
                    (int) Math.Round(0.5 * parameter * angle * Math.Cos(angle)) + random.Next(2, 10));
                yield return result;
            }
        }
    }
}