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
        public Point Center { get; }

        private const double SpiralShift = 2;
        private const double AngleShift = 0.05;

        private double angle = 0;
        public List<Rectangle> Rectangles {get;}

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = GetRectangleInCurrentSpiralPosition(rectangleSize);

            while (AreIntersectingWithPrevious(rectangle))
            {
                angle += AngleShift;
                rectangle = GetRectangleInCurrentSpiralPosition(rectangleSize);
            }

            Rectangles.Add(rectangle);

            return rectangle;
        }

        private Point GetCurrentPositionOnTheSpiral()
        {
            var x = Center.X + (SpiralShift * angle * Math.Cos(angle));
            var y = Center.Y + (SpiralShift * angle * Math.Sin(angle));

            return new Point((int)x, (int)y);
        }

        private Point GetLocationAfterCentering(Point location, Size rectangleSize)
        {
            return new Point(location.X - rectangleSize.Width / 2, location.Y - rectangleSize.Height / 2);
        }

        private bool AreIntersectingWithPrevious(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith);
        }

        private Rectangle GetRectangleInCurrentSpiralPosition(Size rectangleSize)
        {
            var locationOnSpiral = GetCurrentPositionOnTheSpiral();
            var location = GetLocationAfterCentering(locationOnSpiral, rectangleSize);

            return new Rectangle(location, rectangleSize);
        }
    }
}
