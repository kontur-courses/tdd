using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; } = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }
            
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle dimensions must be positive");

            var rectangle = new Rectangle(Center, rectangleSize);
            rectangle = MoveToValidPosition(rectangle);
            rectangle = TryMoveCloserToCenter(rectangle);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private bool RectangleHasCollisions(Rectangle rectangle)
        {
            return Rectangles.Any(r => r.IntersectsWith(rectangle));
        }

        private Rectangle MoveToValidPosition(Rectangle rectangle)
        {
            for (var step = 1; RectangleHasCollisions(rectangle); step++)
            {
                var x = step * Math.Cos(step);
                var y = step * Math.Sin(step);
                rectangle.Location = new Point((int)x + Center.X, (int)y + Center.Y);
            }

            return rectangle;
        }

        private Rectangle TryMoveCloserToCenter(Rectangle rectangle)
        {
            var stepX = 1 * Math.Sign(Center.X - rectangle.Location.X);
            var stepY = 1 * Math.Sign(Center.Y - rectangle.Location.Y);

            var previousLocation = rectangle.Location;
            while (!RectangleHasCollisions(rectangle) && rectangle.X != Center.X + stepX)
            {
                previousLocation = rectangle.Location;
                rectangle = rectangle.Move(stepX, 0);
            }
            rectangle.Location = previousLocation;

            while (!RectangleHasCollisions(rectangle) && rectangle.Y != Center.Y + stepY)
            {
                previousLocation = rectangle.Location;
                rectangle = rectangle.Move(0, stepY);
            }
            rectangle.Location = previousLocation;

            return rectangle;
        }
    }
}
