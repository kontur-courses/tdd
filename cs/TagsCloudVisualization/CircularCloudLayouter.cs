using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Spiral _spiral;
        private List<Rectangle> _rectangles;
        private Point _center;
        
        public CircularCloudLayouter(Point center)
        {
            _center = center;
            _spiral = new Spiral(center, 2);
            _rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize == Size.Empty || rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException("The size must not be equal to or less than 0");
            
            Rectangle rectangle;
            do
            {
                rectangle = new Rectangle(_spiral.NextPoint(), rectangleSize);
            } while (rectangle.IsIntersects(_rectangles));
            
            if(_rectangles.Count != 0)
                rectangle = MoveRectangleToCenter(rectangle);
            
            _rectangles.Add(rectangle);
            return rectangle;
        }
        
        private Rectangle MoveRectangleToCenter(Rectangle newRectangle)
        {
            var shiftX = newRectangle.GetCenter().X < _center.X ? 1 : -1;
            var shiftY = newRectangle.GetCenter().Y < _center.Y ? 1 : -1;
            newRectangle = MoveRectangle(newRectangle, shiftX, shiftY);
            return newRectangle;
        }
        
        private Rectangle MoveRectangle(Rectangle newRectangle, int x, int y)
        {
            var shift = new Size(x, y);
            while (!newRectangle.IsIntersects(_rectangles))
                newRectangle.Location += shift;
            
            newRectangle.Location -= new Size(shift.Width, shift.Height);
            return newRectangle;
        }

    }
}