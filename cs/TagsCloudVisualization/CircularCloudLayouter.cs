using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Spiral _spiral;
        
        public CircularCloudLayouter(Point center, double step = 10)
        {
            _spiral = new Spiral(center, step);
        }

        public Rectangle PutNextRectangle(Size rectangleSize, IReadOnlyList<Rectangle> rectangles)
        {
            if (rectangleSize == Size.Empty || rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("Size must not be equal to 0");
            
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(_spiral.NextPoint(), rectangleSize);
            } while (IsIntersects(rectangle, rectangles)); 
            
            return rectangle;
        }

        private bool IsIntersects(Rectangle newRectangle, IReadOnlyList<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                if (rectangle.IntersectsWith(newRectangle))
                    return true;
            }

            return false;
        }
    }
}