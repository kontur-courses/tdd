using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const double CircleStep = 0.001;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private double circleAngle;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public int Radius => GetLayouterRadius();

        public Point Center { get; }
        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"{nameof(rectangleSize)} should be positive");

            var rectangle = GetNextRectangle(rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                rectangle = CreateRectangleFromAngle(rectangleSize);
                circleAngle++;
            } while (rectangles.Any(innerRectangle => innerRectangle.IntersectsWith(rectangle)));

            return rectangle;
        }

        private Rectangle CreateRectangleFromAngle(Size rectangleSize)
        {
            var rectanglePosition = CreateRectanglePositionFromAngle();

            return new Rectangle(rectanglePosition, rectangleSize).CenterRectangleLocation();
        }

        private Point CreateRectanglePositionFromAngle()
        {
            var x = Center.X + (int) (circleAngle * CircleStep * Math.Cos(circleAngle));
            var y = Center.Y + (int) (circleAngle * CircleStep * Math.Sin(circleAngle));
            return new Point(x, y);
        }

        private int GetLayouterRadius()
        {
            if (rectangles.Any())
                return rectangles
                    .Select(GetRectangleMaxSidePosition)
                    .Max();
            return 0;
        }

        private static int GetRectangleMaxSidePosition(Rectangle rectangle)
        {
            return Math.Max(
                Math.Max(Math.Abs(rectangle.Left), Math.Abs(rectangle.Right)),
                Math.Max(Math.Abs(rectangle.Top), Math.Abs(rectangle.Bottom)));
        }
    }
}