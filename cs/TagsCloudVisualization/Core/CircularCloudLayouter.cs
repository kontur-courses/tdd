using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Core
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        private ArchimedeanSpiral Spiral { get; }
        private Point Center { get; }

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Spiral = new ArchimedeanSpiral(center);
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = GetRectangleWith(rectangleSize);
            while (IsAnyRectangleIntersectWith(nextRectangle))
                nextRectangle = GetRectangleWith(rectangleSize);

            var shiftedNextRectangle = GetShiftedToCenterRectangle(nextRectangle);
            Rectangles.Add(shiftedNextRectangle);
            return shiftedNextRectangle;

            Rectangle GetRectangleWith(Size size) => new Rectangle(Spiral.GetNextPoint(), size);
        }

        private Rectangle GetShiftedToCenterRectangle(Rectangle initialRectangle)
        {
            var minDistanceToCenter = double.MaxValue;
            var shiftedRectangle = initialRectangle;
            var queue = new Queue<Rectangle>();
            queue.Enqueue(shiftedRectangle);

            while (queue.Count != 0)
            {
                var currentRectangle = queue.Dequeue();
                var distanceToCenter = DistanceBetween(currentRectangle.Location, Center);
                if (IsAnyRectangleIntersectWith(currentRectangle) || minDistanceToCenter <= distanceToCenter)
                    continue;
                minDistanceToCenter = distanceToCenter;
                shiftedRectangle = currentRectangle;
                GetNeighboursFor(currentRectangle).ForEach(r => queue.Enqueue(r));
            }

            return shiftedRectangle;

            double DistanceBetween(Point first, Point second) => Math.Sqrt((first.X - second.X) * (first.X - second.X) +
                                                                           (first.Y - second.Y) * (first.Y - second.Y));
        }

        private bool IsAnyRectangleIntersectWith(Rectangle currentRectangle) =>
            Rectangles.Any(r => r.IntersectsWith(currentRectangle));

        private static List<Rectangle> GetNeighboursFor(Rectangle rectangle)
        {
            var neighbours = new (int X, int Y)[] {(1, 0), (0, 1), (-1, 0), (0, -1)};
            return neighbours
                .Select(p => new Rectangle(p.X + rectangle.X, p.Y + rectangle.Y, rectangle.Width, rectangle.Height))
                .ToList();
        }
    }
}