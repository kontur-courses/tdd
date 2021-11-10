using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        public Point Center { get; }
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size Should be positive but was: " +
                                            $"Height: {rectangleSize.Height}" +
                                            $"Width: {rectangleSize.Width}");

            if (rectangles.Count == 0)
                return GetFirstRectangle(rectangleSize);

            var neededRectangle = GetNearestCorrectRectangle(rectangleSize);
            if (neededRectangle != new Rectangle())
            {
                rectangles.Add(neededRectangle);
                return neededRectangle;
            }

            throw new InvalidOperationException("There is no points to place rectangle");
        }

        private IEnumerable<Point> GetNextRectanglePotentialPoints(Size rectangleSize, Rectangle rectangle)
        {
            var potentialPoints = new[]
            {
                new Point(rectangle.Left, rectangle.Top - rectangleSize.Height),
                new Point(rectangle.Right, rectangle.Top),
                new Point(rectangle.Left, rectangle.Bottom),
                new Point(rectangle.Left - rectangleSize.Width, rectangle.Top)
            };

            foreach (var point in potentialPoints)
                yield return point;
        }

        private Rectangle GetFirstRectangle(Size rectangleSize)
        {
            var rectangleX = Center.X - rectangleSize.Width / 2;
            var rectangleY = Center.Y - rectangleSize.Height / 2;
            var upperLeftCorner = new Point(rectangleX, rectangleY);
            var rectangle = new Rectangle(upperLeftCorner, rectangleSize);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNearestCorrectRectangle(Size rectangleSize)
        {
            var potentialPoints = rectangles.SelectMany(rectangle =>
                    GetNextRectanglePotentialPoints(rectangleSize, rectangle));
            var potentialRectangles = potentialPoints.Select(p => new Rectangle(p, rectangleSize))
                    .Where(rect =>
                        rectangles.All(x => !x.IntersectsWith(rect)));
            return potentialRectangles.Aggregate((nearest, current) =>
                    current.GetCenter().DistanceTo(Center) <
                    nearest.GetCenter().DistanceTo(Center)
                        ? current
                        : nearest);
        }
    }
}