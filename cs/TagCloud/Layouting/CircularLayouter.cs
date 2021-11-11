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

        public CircularLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            arrow = new DirectingArrow(center);
        }

        public Point Center { get;}

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);

            var rect = new Rectangle(new Point(Center.X - rectangleSize.Width / 2,
                Center.Y - rectangleSize.Height / 2), rectangleSize);

            while (rect.IsIntersectsWithAny(rectangles))
            {
                arrow.Rotate();
                rect.Location = arrow.GetEndPoint();
                rect.MoveMiddlePointToCurrentLocation();
            }

            rectangles.Add(rect);
            return rect;
        }

        public List<Rectangle> GetRectanglesCopy()
        {
            return new List<Rectangle>(rectangles);
        }

        public int GetCloudBoundaryRadius()
        {
            var boundaryBox = GetRectanglesBoundaryBox();
            var biggestSide = Math.Max(boundaryBox.Width, boundaryBox.Height);

            return (int)Math.Ceiling(biggestSide * (Math.Sqrt(2) / 2));
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