using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class SquareSpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;

        public SquareSpiralGenerator(Point center)
        {
            this.center = center;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            Point currentPoint;
            yield return center;
            yield return new Point(center.X + 1, center.Y);
            yield return currentPoint = new Point(center.X + 1, center.Y - 1);
            var iteration = 1;
            while (true)
            {
                var delta = iteration % 2 == 1 ? 1 : -1;
                for (var i = 0; i < iteration; i++)
                {
                    currentPoint = new Point(currentPoint.X - delta, currentPoint.Y);
                    yield return currentPoint;
                }

                for (var i = 0; i < iteration; i++)
                {
                    currentPoint = new Point(currentPoint.X, currentPoint.Y + delta);
                    yield return currentPoint;
                }

                iteration++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}