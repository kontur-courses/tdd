using System;
using System.Drawing;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualization.Infrastructure.Layout
{
    public class SpiralPlacing : ILayoutStrategy
    {
        private readonly Point center;
        private readonly int angleIncrement;
        private readonly ICollisionDetector<Rectangle> collisionDetector;
        public SpiralPlacing(ICollisionDetector<Rectangle> collisionDetector, Point center, int angleIncrement)
        {
            this.center = center;
            this.angleIncrement = angleIncrement;
            this.collisionDetector = collisionDetector;
        }
        
        public Point GetPlace(Size rectangleSize)
        {
            var rectangle = new Rectangle(Point.Empty, rectangleSize);
            rectangle = MoveAlongSpiral(rectangle);
            rectangle = ShiftTowardsUntilCollision(rectangle);
            return rectangle.Location;
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
                if (!collisionDetector.IsColliding(rectangle))
                    break;
                angle += angleIncrement;
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
                if (collisionDetector.IsColliding(rectangle))
                    break;
            }

            rectangle.Location -= targetTrend; 
            return rectangle;
        }
    }
}