using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly List<(double distance, Rectangle rectangle)> lastRectangles = new();
        private int area = 0;
        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentOutOfRangeException(nameof(rectangleSize));
            if (lastRectangles.Count == 0)
            {
                var location = rectangleSize / 2 * -1;
                var rectangle = new Rectangle(new Point(location + new Size(Center)), rectangleSize);
                lastRectangles.Add((rectangle.GetCenter().DistanceTo(Center), rectangle));
            }
            else
            {
                var rectangle = CreateNextRectangle(rectangleSize);
                lastRectangles.Add((rectangle.GetCenter().DistanceTo(Center), rectangle));
            }

            area += rectangleSize.Width * rectangleSize.Height;

            return lastRectangles[^1].rectangle;
        }

        private Rectangle CreateNextRectangle(Size nextSize)
        {
            var pendingRectangles = new List<(double distance, Rectangle? rectangle)>();
            var minimalDistance = Math.Sqrt((nextSize.Width * nextSize.Height + area) / (Math.PI * 2));
            foreach (var currRectangle in lastRectangles)
            {
                var possiblePositions = new[]
                {
                    new Point(currRectangle.rectangle.Left, currRectangle.rectangle.Top - nextSize.Height),
                    new Point(currRectangle.rectangle.Left, currRectangle.rectangle.Bottom),
                    new Point(currRectangle.rectangle.Right, currRectangle.rectangle.Top),
                    new Point(currRectangle.rectangle.Left - nextSize.Width, currRectangle.rectangle.Top),
                };
                var result = possiblePositions.Select(x => new Rectangle?(new Rectangle(x, nextSize)))
                    .Where(x => x != currRectangle.rectangle)
                    .Select(x => (distance: x.Value.GetCenter().DistanceTo(Center), rectangle: x))
                    .Where(x => x.distance >= minimalDistance)
                    .Where(rectangle => lastRectangles
                        .All(x => !x.rectangle.IntersectsWith(rectangle.rectangle.Value)))
                    .MinBy(x => x.distance);
                if (result.rectangle.HasValue)
                    pendingRectangles.Add(result);
            }

            return pendingRectangles.MinBy(x => x.distance).rectangle.Value;
        }
    }
}
