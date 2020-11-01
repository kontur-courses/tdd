using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle(Point.Empty, rectangleSize);
            rectangle = MoveAlongSpiral(rectangle);
            rectangle = ShiftTowardsUntilCollision(rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }
        
        private Rectangle MoveAlongSpiral(Rectangle rectangle)
        {
            var rectangleOffset = new Size(-rectangle.Width / 2, -rectangle.Height / 2);
            int GetRadius(int angle) => angle;
            var angle = 0;
            
            while (true)
            {
                var possibleLocation = 
                    center 
                    + rectangleOffset
                    + new Size(
                        (int) (Math.Sin(angle) * GetRadius(angle)), 
                        (int) (Math.Cos(angle) * GetRadius(angle)));
                rectangle.Location = possibleLocation;
                if (!rectangles.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle)))
                    break;
                angle+= 1;
            }
            Console.WriteLine($"Rectangle {rectangle.Size} placed {rectangle.Location} at angle {angle}");
            return rectangle;
        }

        private Rectangle ShiftTowardsUntilCollision(Rectangle rectangle)
        {
            var targetTrend = new Size(
                Math.Sign(center.X - rectangle.Location.X - rectangle.Width / 2),
                Math.Sign(center.Y - rectangle.Location.Y - rectangle.Height / 2));
            if (targetTrend == new Size())
                return rectangle;
            
            while (true)
            {
                var possiblePosition = rectangle.Location + targetTrend;
                rectangle.Location = possiblePosition;
                if (rectangles.Any(placedRectangle => placedRectangle.IntersectsWith(rectangle)))
                    break;
            }

            rectangle.Location -= targetTrend; 
            return rectangle;
        }
    }
}