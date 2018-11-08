using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private double circleAngle;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Point Center { get; }
        public IReadOnlyList<Rectangle> Rectangles => rectangles.AsReadOnly();

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle size should be positive");

            return GetNextRectangle(rectangleSize);
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                rectangle = CreateRectangleFromAngle(rectangleSize);
                circleAngle++;
            } while (rectangles.Any(innerRectangle => innerRectangle.IntersectsWith(rectangle)));

            rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle CreateRectangleFromAngle(Size rectangleSize)
        {
            var x = Center.X + (int) (circleAngle * Math.Cos(circleAngle));
            var y = Center.Y + (int) (circleAngle * Math.Sin(circleAngle));
            var rectanglePosition = new Point(x, y);

            return new Rectangle(rectanglePosition, rectangleSize).CenterRectangleLocation();
        }
    }
}