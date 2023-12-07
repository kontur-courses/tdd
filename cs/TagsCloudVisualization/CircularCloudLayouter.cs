using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ICircularCloudLayouter
    {
        private readonly Cloud cloud;
        private readonly IPointDistributor distributor;

        public CircularCloudLayouter(Point center, IPointDistributor type)
        {
            cloud = new Cloud(center);
            distributor = type;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();

            if (cloud.Rectangles.Count == 0)
                return AddToCenterPosition(rectangleSize);

            var rectangle = new Rectangle(FindNewRectanglePosition(rectangleSize), rectangleSize);

            while (HaveIntersection(rectangle))
            {
                rectangle.Location = FindNewRectanglePosition(rectangleSize);
            }

            cloud.Rectangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle AddToCenterPosition(Size rectangleSize)
        {
            var newRectangle = new Rectangle(new Point(cloud.Center.X - rectangleSize.Width / 2,
                cloud.Center.Y - rectangleSize.Height / 2), rectangleSize);

            cloud.Rectangles.Add(newRectangle);

            return newRectangle;
        }

        private Point FindNewRectanglePosition(Size rectangleSize, double deltaAngle = 0.1)
        {
            return distributor.GetPosition(cloud, rectangleSize, 0.1);
        }

        private bool HaveIntersection(Rectangle newRectangle) =>
            cloud.Rectangles.Any(rectangle => rectangle.IntersectsWith(newRectangle));
    }
}