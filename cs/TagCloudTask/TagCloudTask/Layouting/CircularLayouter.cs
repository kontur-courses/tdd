using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagCloudTask.Geometry;

namespace TagCloudTask.Layouting
{
    public class CircularLayouter : ICloudLayouter
    {
        private readonly DirectingArrow _arrow;
        private readonly List<Rectangle> _rectangles;

        public CircularLayouter()
        {
            Center = Point.Empty;
            _rectangles = new List<Rectangle>();
            _arrow = new DirectingArrow(Center);
        }

        public CircularLayouter(Point center)
        {
            Center = center;
            _rectangles = new List<Rectangle>();
            _arrow = new DirectingArrow(center);
        }

        public Point Center { get; }

        public List<Rectangle> GetRectanglesCopy()
        {
            return new List<Rectangle>(_rectangles);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);

            var rect = new Rectangle(Center.MovePoint(-rectangleSize.Width / 2,
                -rectangleSize.Height / 2), rectangleSize);

            while (rect.IntersectsWithAny(_rectangles))
            {
                _arrow.Rotate();
                var arrowEndPoint = _arrow.GetEndPoint();
                rect.Location = arrowEndPoint;
                rect = rect.MoveMiddlePointToCurrentLocation();
            }

            _rectangles.Add(rect);
            return rect;
        }

        public int GetCloudBoundaryRadius()
        {
            return _rectangles.Count == 0
                ? 0
                : (int)Math.Ceiling(_rectangles
                    .Max(rectangle => rectangle.GetLongestDistanceFromPoint(Center)));
        }

        public Size GetRectanglesBoundaryBox()
        {
            if (_rectangles.Count == 0)
                return Size.Empty;

            var width
                = _rectangles.Max(rect => rect.Right) - _rectangles.Min(rect => rect.X);
            var height
                = _rectangles.Max(rect => rect.Bottom) - _rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        private void ThrowIfIncorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of rectangle must be a positive numbers");
        }
    }
}