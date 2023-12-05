using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> rectangles;
        private Point center;
        private Spiral spiral;
        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("the coordinates of the center must be positive numbers");
            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0  || rectangleSize.Height <= 0)
                throw new ArgumentException("the width and height of the rectangle must be positive numbers");
            
            Rectangle nextRengtanle = spiral.GetPossibleNextRectangle(rectangles, rectangleSize);
            rectangles.Add(nextRengtanle);
            return nextRengtanle;
        }

        public List<Rectangle> Rectangles => rectangles;

        public Point Center => center;
    }
}
