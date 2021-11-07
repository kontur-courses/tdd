using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            {
                throw new ArgumentException("Size's width and height must be positive numbers");
            }
            
            var nextPoint = spiral.GetNextPoint();
            var newRectangle = new Rectangle(nextPoint, rectangleSize);

            while (rectangles.Any(newRectangle.IntersectsWith))
            {
                nextPoint = spiral.GetNextPoint();
                newRectangle.Location = nextPoint;
            }
            
            rectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}