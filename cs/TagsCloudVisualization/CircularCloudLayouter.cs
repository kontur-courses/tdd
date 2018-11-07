using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0) throw new ArgumentException("width must be a positive number");
            if (rectangleSize.Height < 0) throw new ArgumentException("height must be a positive number");
            var rectangle = new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2), rectangleSize);

            var phi = 0.0;
            while (IntersectionWithOtherRectangles(rectangle))
            {
                phi += 0.1;
                rectangle.X = center.X + (int)Math.Floor(0.1 * phi * Math.Cos(phi));
                rectangle.Y = center.Y + (int)Math.Floor(0.1 * phi * Math.Sin(phi));
            }

            Rectangles.Add(rectangle);

            return rectangle;
        }

        private bool IntersectionWithOtherRectangles(Rectangle rect) => Rectangles.Exists(rect.IntersectsWith);
    }
}
