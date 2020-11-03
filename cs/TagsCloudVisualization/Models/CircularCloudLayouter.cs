using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Models
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly ArchimedeanSpiral spiral;
        private readonly List<Rectangle> rectangles;
        public IEnumerable<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            this.spiral = new ArchimedeanSpiral(center);
            this.rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            do
            {
                var nextPoint = Point.Round(spiral.GetNextPoint());
                rectangle = new Rectangle(nextPoint, rectangleSize);
            } while (IsIntersectsWithLayout(rectangle));

            if (rectangles.Count > 0)
                rectangle = ShiftToCenter(rectangle);

            rectangles.Add(rectangle);
            return rectangle;
        }

        public Size GetLayoutSize()
        {
            if (rectangles.Count == 0)
                throw new ArgumentException("Layout is empty");

            var deltaX = rectangles
                .Max(rectangle => rectangle.Right) - center.X;
            var deltaY = rectangles
                .Max(rectangle => rectangle.Bottom) - center.Y;
            var width = 2 * Math.Max(center.X, deltaX);
            var height = 2 * Math.Max(center.Y, deltaY);

            return new Size(width, height);
        }

        private Rectangle ShiftToCenter(Rectangle rect)
        {
            var offsetX = center.X - rect.X < 0 ? -1 : 1;
            var offsetY = center.Y - rect.Y < 0 ? -1 : 1;

            rect = ShiftThroughDirection(rect, new Point(offsetX, 0));
            return ShiftThroughDirection(rect, new Point(0, offsetY));
        }

        private Rectangle ShiftThroughDirection(Rectangle rect, Point direction)
        {
            var shiftedRect = rect;

            while (!IsIntersectsWithLayout(rect) &&
                   rect.X != center.X && rect.Y != center.Y)
            {
                shiftedRect = rect;
                var newLocation = new Point(
                    rect.Location.X + direction.X,
                    rect.Location.Y + direction.Y);
                rect = new Rectangle(newLocation, rect.Size);
            }

            return shiftedRect;
        }

        private bool IsIntersectsWithLayout(Rectangle rectangle)
        {
            return rectangles.Any(rectangle.IntersectsWith);
        }
    }
}
