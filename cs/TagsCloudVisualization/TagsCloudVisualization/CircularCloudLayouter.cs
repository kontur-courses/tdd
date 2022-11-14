using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : IRectangleLayouter
    {
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
        }

        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();
        public Point Center { get; }
        private List<Rectangle> rectangles { get; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count == 0)
            {
                var rectangle = new Rectangle(Center - rectangleSize / 2, rectangleSize);
                rectangles.Add(rectangle);
                return rectangle;
            }

            var step = Math.Max(rectangleSize.Height, rectangleSize.Width);
            var startAngle = rectangles.Count;
            foreach (var rectangleCenter in ScatterPointsEvenlyBySpiralLazy(step, startAngle))
            {
                var rectangle = new Rectangle(rectangleCenter - rectangleSize / 2, rectangleSize);
                if (!IntersectsWithExistingRectangles(rectangle))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            }

            throw new InvalidOperationException(
                $"Could not find a place for a rectangle with width {rectangleSize.Width} and height {rectangleSize.Height}");
        }

        private IEnumerable<Point> ScatterPointsEvenlyBySpiralLazy(double step, double startAngle = 0)
        {
            var arg = startAngle;
            var r = step;
            yield return new Point(Center.X + (int)Math.Round(r * Math.Cos(arg)),
                Center.Y + (int)Math.Round(r * Math.Sin(arg)));

            while (true)
            {
                arg += Math.Atan(step / r);
                r = ((arg - startAngle) / 2 / Math.PI - +1) * step;
                yield return new Point(Center.X + (int)Math.Round(r * Math.Cos(arg)),
                    Center.Y + (int)Math.Round(r * Math.Sin(arg)));
            }
        }

        private bool IntersectsWithExistingRectangles(Rectangle newRectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(newRectangle));
        }
    }

}