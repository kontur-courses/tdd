using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles = new();
        private readonly Spiral _spiral;
        public Point Center => _spiral.Center;

        public CircularCloudLayouter(Point center = default)
        {
            _spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Rectangle sizes must be great than zero");

            Rectangle rectangle;

            do
            {
                var point = _spiral.GetNextPoint() - rectangleSize / 2;
                rectangle = new Rectangle(point, rectangleSize);
            } while (IsLayoutIntersectWith(rectangle));

            rectangle = MoveToCenter(rectangle);

            _rectangles.Add(rectangle);

            return rectangle;
        }

        private bool IsLayoutIntersectWith(Rectangle rectangle)
        {
            return _rectangles.Any(rectangle.IntersectsWith);
        }


        public Rectangle[] GetLayout()
        {
            return _rectangles.ToArray();
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            if (_rectangles.Count == 0)
                return rectangle;

            rectangle = ShiftToCenter(rectangle, false);
            rectangle = ShiftToCenter(rectangle, true);
            rectangle = ShiftToCenter(rectangle, false);

            return rectangle;
        }

        private Rectangle ShiftToCenter(Rectangle rectangle, bool isVertical)
        {
            var oldDistance = (rectangle.Location + rectangle.Size / 2).GetDistance(_spiral.Center);
            var oldLocation = rectangle.Location;
            while (true)
            {
                var newLocation = isVertical
                    ? new Point(oldLocation.X, oldLocation.Y + Math.Sign(_spiral.Center.Y - rectangle.Location.Y - rectangle.Size.Height / 2))
                    : new Point(oldLocation.X + Math.Sign(_spiral.Center.X - rectangle.Location.X - rectangle.Size.Width / 2), oldLocation.Y);

                var newDistance = (newLocation + rectangle.Size / 2).GetDistance(_spiral.Center);
                var newRectangle = new Rectangle(newLocation, rectangle.Size);

                if (!IsLayoutIntersectWith(newRectangle))
                    rectangle = newRectangle;

                if (newDistance >= oldDistance)
                    break;

                oldLocation = rectangle.Location;
                oldDistance = newDistance;
            }

            return rectangle;
        }
    }
}