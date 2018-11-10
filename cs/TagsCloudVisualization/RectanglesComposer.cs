using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class RectanglesComposer : IEnumerable<Point>
    {
        private readonly Point center;
        private double phi;
        private int x;
        private int y;

        public RectanglesComposer(Point center)
        {
            this.center = center;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            while (true)
            {
                phi += 0.01;
                x = center.X + (int)Math.Floor(0.01 * phi * Math.Cos(phi));
                y = center.Y + (int)Math.Floor(0.01 * phi * Math.Sin(phi));
                yield return new Point(x, y);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
