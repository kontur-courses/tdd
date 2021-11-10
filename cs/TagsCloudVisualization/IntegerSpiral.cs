using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class IntegerSpiral : IEnumerable<Point>
    {
        private readonly Point center;

        public IntegerSpiral(Point center)
        {
            this.center = center;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            yield return center;

            var spiralSize = 1;
            while (true)
            {
                for (var dx = -spiralSize; dx <= spiralSize; dx++)
                {
                    yield return new Point(center.X + dx, center.Y - spiralSize);
                }

                for (int dy = -spiralSize + 1; dy <= spiralSize; dy++)
                {
                    yield return new Point(center.X + spiralSize, center.Y + dy);
                }

                for (int dx = -spiralSize + 1; dx <= spiralSize; dx++)
                {
                    yield return new Point(center.X - dx, center.Y + spiralSize);
                }

                for (int dy = -spiralSize + 1; dy < spiralSize; dy++)
                {
                    yield return new Point(center.X - spiralSize, center.Y - dy);
                }

                spiralSize++;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}