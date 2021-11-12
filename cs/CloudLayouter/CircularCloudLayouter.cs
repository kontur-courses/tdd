using System;
using System.Collections.Generic;
using System.Drawing;

namespace CloudLayouter
{
    public class CircularCloudLayouter
    {
        private Rectangle canvas;
        private List<Rectangle> rectangles = new();
        private IEnumerator<Point> spiral;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Center point given is in wrong quadrant.");
            
            canvas = GetRectangleAtPosition(center, 2 * new Size(center));
            spiral = new Spiral(center, Math.PI/360, 2).GetEnumerator();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Given rectangle size is negative");

            while (spiral.MoveNext())
            {
                var rectangle = GetRectangleAtPosition(spiral.Current, rectangleSize);
                
                if(rectangle.IntersectsWith(rectangles))
                    continue;
                
                CheckIfRectangleIsOutsideOfCanvas(rectangle);
                
                rectangles.Add(rectangle);
                return rectangle;
            }

            throw new Exception("Given rectangle couldn't be placed");
        }

        private Rectangle GetRectangleAtPosition(Point position, Size rectangleSize)
        {
            position -= rectangleSize / 2;

            return new Rectangle(position, rectangleSize);
        }

        private void CheckIfRectangleIsOutsideOfCanvas(Rectangle rectangle)
        {
            var copy = rectangle;
            
            copy.Intersect(canvas);
            
            if (copy != rectangle)
                throw new Exception("Rectangle was placed out side of canvas");
        }
    }
}