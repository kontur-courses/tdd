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
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(rectangleSize));
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

            return lastRectangles[^1];
        }

        private Rectangle CreateNextRectangle(Size nextSize)
        {
            var pendingRectangles = new List<(double distance, Rectangle? rectangle)>();
            var minimalDistance = Math.Sqrt((nextSize.Width * nextSize.Height + area) / (Math.PI * 2));
            foreach (var currRectangle in lastRectangles)
            {
                var result = GetPossiblePositions(currRectangle, nextSize)
                    .Select(x => new Rectangle?(new Rectangle(x, nextSize)))
                    .Where(x => x != currRectangle)
                    .Select(x => (distance: x.Value.GetCenter().DistanceTo(Center), rectangle: x))
                    .Where(x => x.distance >= minimalDistance)
                    .Where(rectangle => lastRectangles
                        .All(x => !x.IntersectsWith(rectangle.rectangle.Value)))
                    .MinBy(x => x.distance);
                if (result.rectangle.HasValue)
                    pendingRectangles.Add(result);
            }

            return pendingRectangles.MinBy(x => x.distance).rectangle.Value;
        }

        private IEnumerable<Point> GetPossiblePositions(Rectangle anchor, Size size)
        {
            yield return new Point(anchor.Left, anchor.Top - size.Height);
            yield return new Point(anchor.Left, anchor.Bottom);
            yield return new Point(anchor.Right, anchor.Top);
            yield return new Point(anchor.Left - size.Width, anchor.Top);
        }
    }
}
