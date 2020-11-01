using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CircularCloudLayouter
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        public readonly Point Center;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            _rectangles = new List<Rectangle>();
        }
        
        public Rectangle GetCurrentRectangle => _rectangles.Last();

        public List<Rectangle> GetRectangles => _rectangles;

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height == 0 || rectangleSize.Width == 0)
                throw new ArgumentException("Size width and height must be not zero");
        }
    }
}