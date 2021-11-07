using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public readonly List<Rectangle> Rectangles;

        private Point nextPosition;
        
        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size params should be positive");
            
            var position = Center;
            if (Rectangles.Count != 0)
                position = nextPosition;
            
            var rect = new Rectangle(position, rectangleSize);
            Rectangles.Add(rect);
            
            nextPosition = position + rectangleSize;
            
            return rect;
        }
    }
}