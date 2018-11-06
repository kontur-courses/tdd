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
            spiralGenerator = new RoundSpiralGenerator(center, 3.6).GetEnumerator();
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
                //var step = (rectangleSize.Height + rectangleSize.Width) / 20; TODO: remove this or rework
                for (var i = 0; i < 1; i++)
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

    internal class RoundSpiralGenerator : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly double k;

        public RoundSpiralGenerator(Point center, double k)
        {
            this.center = center;
            this.k = k;
        }

        private Point PolarToCartesian(double r, double phi)
        {
            var x = r * Math.Cos(phi) + center.X;
            var y = r * Math.Sin(phi) + center.Y;
            return new Point((int)x, (int)y);
        }
        public IEnumerator<Point> GetEnumerator()
        {
            var phi = 0d;
            int step = 16;
            var dPhi = Math.PI / step;
            //r = k*ф;
            var r = 0d;
            yield return PolarToCartesian(r, phi);
            while (true)
            {
                phi += dPhi;
                r = k * phi;
                yield return PolarToCartesian(r, phi);
            }

        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

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
