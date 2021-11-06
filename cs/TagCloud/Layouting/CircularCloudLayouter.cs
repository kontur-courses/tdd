using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud.Layouting
{
    public class CircularCloudLayouter : ICloudLayouter
    {
        private const int DefaultSpaceBetweenRectangles = 5;

        private readonly Point center;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);
            var rectangle = new Rectangle(Point.Empty, rectangleSize);

            if (rectangles.Count == 0)
            {
                rectangle.Location = GetOffsetFromCenter(rectangleSize);
            }
            else
            {
                var prevRect = rectangles[rectangles.Count - 1];
                rectangle.Location =
                    new Point(prevRect.Location.X + prevRect.Width + DefaultSpaceBetweenRectangles,
                        prevRect.Location.Y + prevRect.Height + DefaultSpaceBetweenRectangles);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        public Point GetCloudCenter()
        {
            return center;
        }

        public Size GetRectanglesBoundaryBox()
        {
            var width
                = rectangles.Max(rect => rect.X + rect.Width)
                  - rectangles.Min(rect => rect.X);

            var height
                = rectangles.Max(rect => rect.Y + rect.Height)
                  - rectangles.Min(rect => rect.Y);

            return new Size(width, height);
        }

        public int GetCloudRadius()
        {
            var boundaryBox = GetRectanglesBoundaryBox();
            return Math.Max(boundaryBox.Width, boundaryBox.Height) / 2;
        }

        public List<Rectangle> GetRectangles()
        {
            return new List<Rectangle>(rectangles);
        }

        private Point GetOffsetFromCenter(Size rectangleSize)
        {
            return new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2);
        }

        private void ThrowIfIncorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of rectangle must be a positive numbers");
        }
    }
}