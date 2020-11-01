using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly HashSet<Point> usedPoints;

        public Spiral(Point center)
        {
            this.center = center;
            usedPoints = new HashSet<Point>();
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
                    if (!usedPoints.Contains(currentPoint))
                        yield return currentPoint;
                }

                for (var i = 0; i < lineLength; i++)
                {
                    currentPoint = Point.Add(currentPoint, new Size(0, sign));
                    if (!usedPoints.Contains(currentPoint))
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

        public void AddUsedPoints(List<Point> points)
        {
            usedPoints.UnionWith(points);
        }
    }
}