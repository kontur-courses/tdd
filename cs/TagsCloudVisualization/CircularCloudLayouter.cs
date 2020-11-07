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
                    var offset = LayouterTools.GetOffset(rectangleSize, rectangle, direction);
                    var location = new Point(rectangle.X + offset.X, rectangle.Y + offset.Y);
                    var currentRectangleCenter = new Point(location.X + rectangleSize.Width / 2,
                        location.Y + rectangleSize.Height / 2);
                    var distance = LayouterTools.CalculateDistance(currentRectangleCenter, Center);
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

        
    }
}