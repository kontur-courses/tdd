using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public readonly List<Rectangle> Rectangles;
        private readonly List<Direction> directions;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            directions = new List<Direction> {Direction.Top, Direction.Right, Direction.Bottom, Direction.Left};
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangle(rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle size should be positive.");

            if (Rectangles.Count == 0)
                return new Rectangle
                {
                    Size = rectangleSize,
                    Location = new Point(Center.X - rectangleSize.Width / 2, Center.Y - rectangleSize.Height / 2)
                };

            var bestDistance = double.MaxValue;
            var bestRectangle = new Rectangle();

            foreach (var rectangle in Rectangles)
            {
                foreach (var direction in directions)
                {
                    var offset = GetOffset(rectangleSize, rectangle, direction);
                    var location = new Point(rectangle.X + offset.X, rectangle.Y + offset.Y);
                    var currentRectangleCenter = new Point(location.X + rectangleSize.Width / 2,
                        location.Y + rectangleSize.Height / 2);
                    var distance = CalculateDistance(currentRectangleCenter, Center);
                    var currentRectangle = new Rectangle {Size = rectangleSize, Location = location};

                    if (!(distance < bestDistance) || IntersectWithOtherRectangles(currentRectangle)) continue;
                    bestDistance = distance;
                    bestRectangle = currentRectangle;
                }
            }

            return bestRectangle;
        }

        private bool IntersectWithOtherRectangles(Rectangle rectangle)
        {
            return Rectangles.Any(previous => previous.IntersectsWith(rectangle));
        }

        private Point GetOffset(Size rectangleSize, Rectangle previous, Direction direction)
        {
            return direction switch
            {
                Direction.Top => new Point(0, previous.Height),
                Direction.Right => new Point(previous.Width, previous.Height - rectangleSize.Height),
                Direction.Bottom => new Point(previous.Width - rectangleSize.Width, -rectangleSize.Height),
                Direction.Left => new Point(-rectangleSize.Width, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private double CalculateDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }
    }
}