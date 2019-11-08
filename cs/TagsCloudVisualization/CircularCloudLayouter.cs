using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        public IEnumerable<Rectangle> Rectangles => new List<Rectangle>(rectangles);

        private readonly Spiral spiral = new Spiral(0.1);
        private double spiralParam;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (rectangles.Count == 0)
            {
                var location = new Point(Center.X - rectangleSize.Width / 2,
                    Center.Y - rectangleSize.Height / 2);
                rectangle = new Rectangle(location, rectangleSize);
            }
            else
            {
                rectangle = new Rectangle(new Point(0, 0), rectangleSize);
                PlaceOnSpiral(ref rectangle);
                SealInLayout(ref rectangle);
            }

            rectangles.Add(rectangle);

            return rectangle;
        }

        private void PlaceOnSpiral(ref Rectangle rectangle)
        {
            while (RectangleIntersectsWithLayout(rectangle))
            {
                var newRectCenter = spiral.Calculate(spiralParam);
                rectangle.Location = new Point(newRectCenter.X - rectangle.Size.Width / 2,
                                               newRectCenter.Y - rectangle.Size.Height / 2);
                spiralParam += 0.1;
            }
        }

        private void SealInLayout(ref Rectangle rectangle)
        {
            var rectCenter = rectangle.GetCenter();
            var dx = Math.Sign(Center.X - rectCenter.X);
            var dy = Math.Sign(Center.Y - rectCenter.Y);

            while (true)
            {
                var prevLocation = rectangle.Location;
                rectangle.X += dx;
                rectangle.Y += dy;
                if (!RectangleIntersectsWithLayout(rectangle))
                    continue;
                rectangle.Location = prevLocation;
                break;
            }
        }

        private bool RectangleIntersectsWithLayout(Rectangle rectangle)
        {
            return rectangles.Select(rectangle.IntersectsWith).Any(r => r);
        }
    }

    public class Spiral
    {
        private readonly Func<double, Point> func;

        public Spiral(double coefficient)
        {
            func = t => new Point(Convert.ToInt32(coefficient * t * Math.Sin(t)),
                                  Convert.ToInt32(coefficient * t * Math.Cos(t)));
        }

        public Point Calculate(double param)
        {
            return func(param);
        }
    }

    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                rect.Top + rect.Height / 2);
        }
    }
}