using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Point CENTER;
        private readonly List<Rectangle> rectangles;
        private const int DEFAULT_SPACE_BETWEEN_RECTANGLES = 5;

        public CircularCloudLayouter(Point center)
        {
            CENTER = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            ThrowIfIncorrectSize(rectangleSize);
            var rectangle = new Rectangle(Point.Empty, rectangleSize);

            if (rectangles.Count == 0)
                rectangle.Location = GetOffsetFromCenter(rectangleSize);
            else
            {
                var prevRect = rectangles[rectangles.Count - 1];
                rectangle.Location =
                    new Point(prevRect.Location.X + prevRect.Width + DEFAULT_SPACE_BETWEEN_RECTANGLES,
                        prevRect.Location.Y + prevRect.Height + DEFAULT_SPACE_BETWEEN_RECTANGLES);
            }

            rectangles.Add(rectangle);
            return rectangle;
        }

        public Point GetCenterPoint()
        {
            return CENTER;
        }

        public Point GetOffsetFromCenter(Size rectangleSize)
        {
            return new Point(CENTER.X - rectangleSize.Width / 2, CENTER.Y - rectangleSize.Height / 2);
        }

        private void ThrowIfIncorrectSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of rectangle must be a positive numbers");
        }
    }
}