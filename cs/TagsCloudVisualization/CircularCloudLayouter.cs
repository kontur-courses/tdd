using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly List<Rectangle> lastRectangles = new();
        private int area = 0;
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width == 0 || rectangleSize.Height == 0)
                throw new ArgumentOutOfRangeException(nameof(rectangleSize));
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                rectangleSize = rectangleSize.Abs();
            Rectangle rectangle;
            if (lastRectangles.Count == 0)
            {
                var location = rectangleSize / 2 * -1;
                rectangle = new Rectangle(new Point(location + new Size(Center)), rectangleSize);
            }
            else
            {
                rectangle = CreateNextRectangle(rectangleSize);
            }

            lastRectangles.Add(rectangle);
            area += rectangleSize.Width * rectangleSize.Height;

            return rectangle;
        }

        private Rectangle CreateNextRectangle(Size nextSize)
        {
            var pendingRectangles = new List<(double distance, Rectangle rectangle)>();
            var distanceCoefficent = 1 - 1 / (0.25 * lastRectangles.Count * lastRectangles.Count + 1);
            var minimalDistance = Math.Sqrt((nextSize.Width * nextSize.Height + area) / (Math.PI * 2)) * distanceCoefficent;
            var result = lastRectangles.SelectMany(currRectangle => GetPossiblePositions(currRectangle, nextSize))
                .Select(x => new Rectangle(x, nextSize))
                .Select(x => (distance: x.GetCenter().DistanceTo(Center), rectangle: x))
                .Where(x => x.distance >= minimalDistance)
                .Where(rectangle => lastRectangles
                    .All(x => !x.IntersectsWith(rectangle.rectangle)))
                .MinBy(x => x.distance);

            return result.rectangle;
        }

        private static IEnumerable<Point> GetPossiblePositions(Rectangle anchor, Size size)
        {
            yield return new Point(anchor.Left + (anchor.Width - size.Width) / 2, anchor.Top - size.Height);
            yield return new Point(anchor.Left + (anchor.Width - size.Width) / 2, anchor.Bottom);
            yield return new Point(anchor.Right, anchor.Top + (anchor.Height - size.Height) / 2);
            yield return new Point(anchor.Left - size.Width, anchor.Top + (anchor.Height - size.Height) / 2);
        }
    }
}
