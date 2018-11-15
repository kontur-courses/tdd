using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Sequences
{
    public class Spiral : IPointSequence
    {
        public int StepsCount { get; }
        private int angle;
        public IEnumerable<Point> GetPoints()
        {
            while (true)
            {
                var angleRads = angle / (2 * Math.PI);
                var r = StepsCount / (2 * Math.PI) * angleRads;
                var x = (int)(r * Math.Cos(angleRads));
                var y = (int)(r * Math.Sin(angleRads));
                yield return new Point(x, y);
                angle++;
            }
        }

        public Spiral(int step = 1)
        {
            if (step <= 0)
                throw new ArgumentException($"{nameof(step)} should be larger than 0");
            StepsCount = step;
        }
    }
}