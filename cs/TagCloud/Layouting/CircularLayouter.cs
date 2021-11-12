using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloud.Geometry;

namespace TagCloud.Layouting
{
    public class CircularLayouter : ICloudLayouter
    {
        private readonly DirectingArrow arrow;
        private readonly List<Rectangle> rectangles;

        public CircularLayouter()
        {
            Center = Point.Empty;
            rectangles = new List<Rectangle>();
            arrow = new DirectingArrow(Center);
        }

        public CircularLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            arrow = new DirectingArrow(center);
        }

        public Point Center { get; }

        public List<Rectangle> GetRectanglesCopy()
        {
            return new List<Rectangle>(rectangles);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);

            var rect = new Rectangle(new Point(Center.X - rectangleSize.Width / 2,
                Center.Y - rectangleSize.Height / 2), rectangleSize);

            while (rect.IntersectsWithAny(rectangles))
            {
                arrow.Rotate();
                var arrowEndPoint = arrow.GetEndPoint();
                rect.Location = arrowEndPoint;
                rect.MoveMiddlePointToCurrentLocation();
            }

            rectangles.Add(rect);
            return rect;
        }

        public int GetCloudBoundaryRadius()
        {
            return rectangles.Count == 0
                ? 0
                : (int)Math.Ceiling(rectangles
                    .Max(rectangle => rectangle.GetLongestDistanceFromPoint(Center)));
        }

        public Size GetRectanglesBoundaryBox()
        {
            if (rectangles.Count == 0)
                return Size.Empty;

            var width
                = rectangles.Max(rect => rect.Right) - rectangles.Min(rect => rect.X);
            var height
                = rectangles.Max(rect => rect.Bottom) - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        private void ThrowIfIncorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of rectangle must be a positive numbers");
        }
    }
}