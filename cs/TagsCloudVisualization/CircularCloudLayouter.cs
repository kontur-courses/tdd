using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Cloud cloud;
        private readonly int step = 1;
        private double angle;

        public CircularCloudLayouter(Point center)
        {
            cloud = new Cloud(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();

            var rectangle = new Rectangle(FindNewRectanglePosition(), rectangleSize);

            while (HaveIntersection(rectangle))
            {
                rectangle.Location = FindNewRectanglePosition();
            }

            cloud.Rectangles.Add(rectangle);

            return rectangle;
        }

        private Point FindNewRectanglePosition(double deltaAngle = 0.1)
        {
            angle += deltaAngle;
            var k = step / (2 * Math.PI);
            var radius = k * angle;

            var position = new Point(
                cloud.Center.X + (int)(Math.Cos(angle) * radius),
                cloud.Center.Y + (int)(Math.Sin(angle) * radius));

            return position;
        }

        private bool HaveIntersection(Rectangle newRectangle) =>
            cloud.Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
    }
}