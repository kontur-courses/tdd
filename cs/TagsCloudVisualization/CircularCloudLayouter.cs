using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly IEnumerator<Point> spiralGenerator;

        public CircularCloudLayouter(Point center)
        {
            spiralGenerator = new SpiralGenerator(center).GetEnumerator();
            spiralGenerator.MoveNext();
        }

        public IEnumerable<Rectangle> Rectangles => rectangles.AsEnumerable();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("size has non positive parts");

            var nextPosition = spiralGenerator.Current;
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (rectangles.Exists(rect => rectangle.IntersectsWith(rect)))
            {
                var step = (rectangleSize.Height + rectangleSize.Width )/ 20;
                for (var i = 0; i < step; i++)
                    spiralGenerator.MoveNext();
                nextPosition = spiralGenerator.Current;
                rectangle = new Rectangle(nextPosition, rectangleSize);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        public void PutNextRectangles(IEnumerable<Size> rectangles)
        {
            foreach (var rectangle in rectangles) PutNextRectangle(rectangle);
        }
    }

    internal class SpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;

        public SpiralGenerator(Point center)
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
