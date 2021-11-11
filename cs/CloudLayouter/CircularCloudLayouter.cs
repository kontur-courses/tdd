using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudLayouter
{
    public class CircularCloudLayouter
    {
        private Point center;
        private Rectangle canvas;
        private List<Rectangle> rectangles = new();
        private Spiral spiral;
        private int iteration;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Center point given is in wrong quadrant.");
            
            this.center = center;
            canvas = GetRectangleAtPosition(center, 2 * new Size(center));
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Given rectangle size is negative");

            foreach (var point in spiral.Skip(iteration))
            {
                iteration++;
                var rectangle = GetRectangleAtPosition(point, rectangleSize);
                
                if(rectangle.IntersectsWith(rectangles))
                    continue;

                rectangle = MoveCloserToCenter(rectangle);
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
        
        //Не успел вынести общий while в отдельный медот
        private Rectangle MoveCloserToCenter(Rectangle rectangle)
        {
            var directionX = Math.Sign(center.X - rectangle.GetCenter().X); 
            var directionY = Math.Sign(center.Y - rectangle.GetCenter().Y);
            
            while (directionX != 0 && !rectangle.IntersectsWith(rectangles) && center.X != rectangle.GetCenter().X)
                rectangle.Offset(directionX, 0);

            while (directionY != 0 && !rectangle.IntersectsWith(rectangles) && center.Y != rectangle.GetCenter().Y)
                rectangle.Offset(0, directionY);
            
            rectangle.Offset(-1 * directionX, -1 * directionY);

            return rectangle;
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