using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Put rectangles in cloud in order of distance from center,
    /// using the spiral of Archimedes
    /// </summary>
    class CircularCloudLayouter
    {
        private readonly Point center;

        private const double SpiralShift = 2;
        private const double AngleShift = 0.01;

        private double angle = 0;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

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
            return rectangles.Any(currentRectangle => currentRectangle.IntersectsWith(rectangle));
        }

        private Rectangle GetRectangleInCurrentSpiralPosition(Size rectangleSize)
        {
            var locationOnSpiral = GetCurrentPositionOnTheSpiral();
            var location = GetLocationAfterCentering(locationOnSpiral, rectangleSize);

            return new Rectangle(location, rectangleSize);
        }
    }
}
