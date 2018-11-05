using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayout : ICircularCloudLayout
    {
        private readonly List<Rectangle> rectangles;
        private readonly Point centerPoint;
        private double angle;

        public CircularCloudLayout(Point centerPoint)
        {
            rectangles = new List<Rectangle>();
            this.centerPoint = centerPoint;
        }

        private IEnumerable<Point> GetPoints()
        {
            while (true)
            {
                if (rectangles.Count == 0)
                {
                    yield return centerPoint;
                }

                var angleInRadians = angle * Math.PI / 180.0;
                var x = angleInRadians * Math.Cos(angleInRadians);
                var y = angleInRadians * Math.Sin(angleInRadians);

                yield return new Point((int)x, (int)y);

                angle++;
            }
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException(
                    "Width and height of rectangle have to be > 0",
                    nameof(rectangleSize));
            }

            var points = GetPoints();

            foreach (var point in points)
            {
                var rectangle = new Rectangle(point, rectangleSize);

                if (rectangle.IntersectsWith(rectangles))
                {
                    continue;
                }

                rectangles.Add(rectangle);

                return rectangle;
            }

            throw new InvalidOperationException(
                $"{nameof(GetPoints)} method didn't return new point by undefined reason.");
        }
    }
}