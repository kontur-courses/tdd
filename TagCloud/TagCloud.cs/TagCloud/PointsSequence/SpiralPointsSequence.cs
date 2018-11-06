using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class SpiralPointsSequence : IPointsSequence
    {
        private double alpha;
        private readonly double step;
        private readonly IEnumerator<Point> enumerator;

        public SpiralPointsSequence(double step)
        {
            if (step <= 0) throw new ArgumentException("step can't be negative or zero");
            this.step = step;
            enumerator = Sequence().GetEnumerator();
        }

        public Point GetNextPoint()
        {
            enumerator.MoveNext();
            return enumerator.Current;
        }

        public void Reset()
        {
            alpha = 0;
        }

        private IEnumerable<Point> Sequence()
        {
            while (true)
            {
                yield return new Point
                {
                    X = (int)(step / (Math.PI * 2) * alpha * Math.Cos(alpha)),
                    Y = (int)(step / (Math.PI * 2) * alpha * Math.Sin(alpha))
                };
                alpha++;
            }
        }
    }
}