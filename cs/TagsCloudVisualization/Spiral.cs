using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IEnumerable<Point>
    {
        private readonly Point center;

        public Spiral(Point center)
        {
            this.center = center;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            var currentPoint = center;
            yield return currentPoint;
            var lineLength = 1;
            var sign = -1;
            while (true)
            {
                for (var i = 0; i < lineLength; i++)
                {
                    currentPoint = Point.Add(currentPoint, new Size(sign, 0));
                    yield return currentPoint;
                }

                for (var i = 0; i < lineLength; i++)
                {
                    currentPoint = Point.Add(currentPoint, new Size(0, sign));
                    yield return currentPoint;
                }

                sign *= -1;
                lineLength++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}