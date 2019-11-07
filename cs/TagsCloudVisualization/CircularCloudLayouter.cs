using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const int DefaultDistance = 20;

        private readonly Point center;
        private readonly CoordinateGenerator generator;
        private readonly Stack<Rectangle> rectangles;

        public CircularCloudLayouter(Point center) :
            this(center, new ArchimedeanSpiralGenerator(DefaultDistance))
        { }

        public CircularCloudLayouter(Point center, CoordinateGenerator generator)
        {
            this.center = center;
            this.generator = generator;
            rectangles = new Stack<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var shiftPoint = new Point(
                center.X - rectangleSize.Width / 2,
                center.Y - rectangleSize.Height / 2
            );
            foreach (var point in generator.GeneratePoints())
            {
                var location = point.Add(shiftPoint);
                var rectangle = new Rectangle(location, rectangleSize);
                if (IntersectsWithOthers(rectangle))
                {
                    continue;
                }
                if (rectangles.Count > 0)
                {
                    rectangle = MoveToCenter(rectangle);
                }
                rectangles.Push(rectangle);
                return rectangle;
            }
            return Rectangle.Empty;
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            Rectangle result;
            var next = rectangle;
            do
            {
                result = next;
                next = MoveToCenterByHorizontally(next);
                next = MoveToCenterByVertically(next);
            } while (result != next);
            return result;
        }

        private Rectangle MoveToCenterByHorizontally(Rectangle rectangle)
        {
            var next = rectangle;
            var dx = Math.Sign(center.X - rectangle.X);
            do
            {
                rectangle = next;
                next.X += dx;
            } while (next.X != center.X && !IntersectsWithOthers(next));

            return rectangle;
        }

        private Rectangle MoveToCenterByVertically(Rectangle rectangle)
        {
            var next = rectangle;
            var dy = Math.Sign(center.Y - rectangle.Y);
            do
            {
                rectangle = next;
                next.Y += dy;
            } while (next.Y != center.Y && !IntersectsWithOthers(next));

            return rectangle;
        }

        private bool IntersectsWithOthers(Rectangle rectangle) =>
            rectangles.Any(rectangle.IntersectsWith);
    }
}
