using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly SpiralTrack spiralTrack;
        private readonly List<Rectangle> pastRectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            var step = 0.5;
            spiralTrack = new SpiralTrack(center, step);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextRectangle = PutNextRectangleBySpiralTrack(rectangleSize);
            //nextRectangle = PullToCenter(nextRectangle);
            pastRectangles.Add(nextRectangle);
            return nextRectangle;
        }

        public Rectangle PullToCenter(Rectangle rectangle)
        {
            var pulledRectangle = rectangle;
            var rectangleCenter = rectangle.GetCenter();

            var dx = rectangleCenter.X - center.X;
            while (TryHorizontalMove(pulledRectangle, dx))
                dx = rectangleCenter.X - center.X;

            var dy = rectangleCenter.Y - center.Y;
            while (TryVerticalMove(pulledRectangle, dy))
                dy = rectangleCenter.Y - center.Y;

            return pulledRectangle;
        }

        public bool TryHorizontalMove(Rectangle rectangle, int dx)
        {
            var newRectangle = new Rectangle(rectangle.X + dx, rectangle.Y, 
                rectangle.Width, rectangle.Height);

            if (NotIntersectWithPastRectangles(newRectangle))
            {
                rectangle.X += dx;
                return true;
            }

            return false;
        }

        public bool TryVerticalMove(Rectangle rectangle, int dy)
        {
            var newRectangle = new Rectangle(rectangle.X, rectangle.Y + dy,
                rectangle.Width, rectangle.Height);

            if (NotIntersectWithPastRectangles(newRectangle))
            {
                rectangle.Y += dy;
                return true;
            }

            return false;
        }

        private bool NotIntersectWithPastRectangles(Rectangle rectangle) =>
            !pastRectangles.Any(rect => rect.IntersectsWith(rectangle));

        private Rectangle PutNextRectangleBySpiralTrack(Size rectangleSize)
        {
            while (true)
            {
                var point = spiralTrack.GetNextPoint();

                var location = new Point(
                    point.X - rectangleSize.Width / 2,
                    point.Y - rectangleSize.Height / 2);

                var rectangle = new Rectangle(location, rectangleSize);
                if (NotIntersectWithPastRectangles(rectangle))
                    return rectangle;
            }
        }
    }
}
