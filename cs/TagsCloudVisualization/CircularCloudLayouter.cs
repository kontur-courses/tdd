using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Size Size { get; }
        public Point Center { get; }
        private readonly List<Rectangle> rectangles;
        private double spiralAngle;
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Size = new Size(center.X * 2, center.Y * 2);
            rectangles = new List<Rectangle>();
        }

        public List<Rectangle> GetRectangles() => rectangles;
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 1 || rectangleSize.Width < 1)
                throw new ArgumentException("Размер прямоугольника должен быть больше 0");

            var rectangle = CreateNewRectangle(rectangleSize);

            while (rectangles.Any(e => e.IntersectsWith(rectangle)))
            {
                spiralAngle++;
                rectangle = CreateNewRectangle(rectangleSize);
            }
            rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle CreateNewRectangle(Size rectangleSize)
        {
            var x = Center.X + (int)(spiralAngle * Math.Cos(spiralAngle));
            var y = Center.Y + (int)(spiralAngle * Math.Sin(spiralAngle));

            var rectangleLocation = new Point(x, y);

            return new Rectangle(rectangleLocation, rectangleSize);
        }
    }
}
