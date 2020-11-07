using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudTag
{
    public class CircularCloudLayouter
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
        }

        public CircularCloudLayouter(int centerX, int centerY) : this(new Point(centerX, centerY))
        {
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException();

            var rectangleToAdd = new Rectangle {Size = rectangleSize};

            do
                rectangleToAdd = rectangleToAdd.SetCenter(spiral.GetNextPoint());
            while (rectangles.Any(rectangle => rectangle.IntersectsWith(rectangleToAdd)));

            rectangles.Add(rectangleToAdd);
            return rectangleToAdd;
        }

        public Rectangle[] GetRectangles()
        {
            return rectangles.ToArray();
        }
    }
}