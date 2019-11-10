using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class ArchimedeanSpiral : IEnumerable<Point>
    {
        private readonly Random random = new Random(42);

        public ArchimedeanSpiral(Point center, int parameter = 1)
        {
            this.parameter = parameter;
            angle = center.X * center.X + center.Y * center.Y;
        }

        private readonly int parameter;
        private double angle;

        public IEnumerable<Point> GetNewPointLazy()
        {
            while (true)
            {
                angle += 0.0001;
                var result = new Point((int) Math.Round(parameter * angle * Math.Sin(angle)) + random.Next(2, 10),
                    (int) Math.Round(0.6 * parameter * angle * Math.Cos(angle)) + random.Next(2, 10));
                yield return result;
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}