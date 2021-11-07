using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles;
        private readonly Spiral spiral;
        public readonly Point Center;
        public IReadOnlyCollection<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiral = Spiral.Create(center);
            rectangles = new();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0
                || rectangleSize.Height <= 0)
                throw new ArgumentException("Size values must be positive numbers");

            var rectangle = GetNextRectangle(rectangleSize);
            rectangle = Optimize(rectangle);
            rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            var rectangle = Rectangle.Empty;
            while (IsIncorrectLocation(rectangle))
            {
                var location = spiral.GetNext() - rectangleSize / 2;
                rectangle = new Rectangle(location, rectangleSize);
            }
            return rectangle;
        }

        private bool IsIncorrectLocation(Rectangle rectangle)
        {
            return rectangle.IsEmpty ? true : rectangles.Any(rectangle.IntersectsWith);
        }

        private Rectangle Optimize(Rectangle rectangle)
        {
            if (rectangles.Count == 0)
                return rectangle;

            rectangle = GetOptimizedRectangle(rectangle);

            return rectangle;
        }

        private Rectangle GetOptimizedRectangle(Rectangle rectangle)
        {
            var distance = Center.GetDistance(rectangle.Location + rectangle.Size / 2);

            while (true)
            {
                var newLocation = new Point(CalculateOptimizedPosition(true, rectangle),
                    CalculateOptimizedPosition(false, rectangle));

                var newDistance = Center.GetDistance(newLocation + rectangle.Size / 2);
                var newRectangle = new Rectangle(newLocation, rectangle.Size);

                if (newDistance >= distance
                    || IsIncorrectLocation(newRectangle))
                    break;

                rectangle = newRectangle;
                distance = newDistance;
            }

            return rectangle;
        }

        private int CalculateOptimizedPosition(bool isX, Rectangle rectangle)
        {
            if (isX)
                return rectangle.Location.X
                    + Math.Sign(Center.X - rectangle.Location.X - rectangle.Size.Width / 2);
            return rectangle.Location.Y
                + Math.Sign(Center.Y - rectangle.Location.Y - rectangle.Size.Height / 2);
        }

        public static CircularCloudLayouter Generate(Point center, int rectCount)
        {
            var layouter = new CircularCloudLayouter(center);
            var random = new Random();

            for (var i = 0; i < rectCount; i++)
                layouter.PutNextRectangle(
                    new Size(random.Next(65, 100), random.Next(30, 60)));
            return layouter;
        }
    }
}
