using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudTag
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var x = center.X - rectangleSize.Width / 2;
            var y = center.Y - rectangleSize.Height / 2;
            var rectangleToAdd = new Rectangle(new Point(x, y), rectangleSize);
            while (rectangles.Any(rectangle => rectangle.IntersectsWith(rectangleToAdd)))
            {
                rectangleToAdd.Location = spiral.GetNextPoint();
            }

            rectangles.Add(rectangleToAdd);
            return rectangles.Last();
        }

        public IEnumerable<Rectangle> GetRectangles()
        {
            return rectangles;
        }
    }
}