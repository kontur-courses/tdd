using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point center;

        private const double SpiralShift = 5;
        private const double AngleShift = 0.1;

        private double angle = 0;
        private List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangleInCurrentSpiralPosition(rectangleSize);

            while (AreIntersectingWithPrevious(rectangle))
            {
                angle += AngleShift;
                rectangle = GetRectangleInCurrentSpiralPosition(rectangleSize);
            }

            rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetCurrentPositionOnTheSpiral()
        {
            var x = center.X + (SpiralShift * angle * Math.Cos(angle));
            var y = center.Y + (SpiralShift * angle * Math.Sin(angle));

            return new Point((int)x, (int)y);
        }

        private Point GetLocationAfterCentering(Point location, Size rectangleSize)
        {
            return new Point(location.X - rectangleSize.Width / 2, location.Y - rectangleSize.Height / 2);
        }

        private bool AreIntersectingWithPrevious(Rectangle rectangle)
        {
            foreach (var rectangleToCheck in rectangles)
            {
                if (rectangleToCheck.IntersectsWith(rectangle))
                {
                    return true;
                }
            }

            return false;
        }

        private Rectangle GetRectangleInCurrentSpiralPosition(Size rectangleSize)
        {
            var locationOnSpiral = GetCurrentPositionOnTheSpiral();
            var location = GetLocationAfterCentering(locationOnSpiral, rectangleSize);

            return new Rectangle(location, rectangleSize);
        }
    }
}
