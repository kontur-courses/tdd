using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly ISpiral spiral;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(ISpiral spiral)
        {
            this.spiral = spiral;
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
                newRectangle = new Rectangle(nextPoint, rectangleSize);
            }

            rectangles.Add(newRectangle);
            return newRectangle;
        }
    }
}