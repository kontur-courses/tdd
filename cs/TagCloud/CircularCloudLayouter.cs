using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }

        public List<Rectangle> Reactangles { get; }

        private SpiralPointGenerator spiralGenerator;

        public CircularCloudLayouter() : this(new Point())
        {
        }

        public CircularCloudLayouter(Point center)
        {
            Center = new Point(center.X, center.Y);
            Reactangles = new List<Rectangle>();
            spiralGenerator = new SpiralPointGenerator(Center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 1 || rectangleSize.Width < 1)
                throw new ArgumentException("");
            Rectangle rectangle;
            do
            {
                rectangle = GetNextRectangle(rectangleSize);
            }
            while (Reactangles.Any(r => r.IntersectsWith(rectangle)));
            Reactangles.Add(rectangle);

            return rectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize) =>
            new Rectangle(GetNextRectanglePoint(rectangleSize), rectangleSize);

        private Point GetNextRectanglePoint(Size rectangleSize)
        {
            var reactangleCenter = GetCenterPointFor(rectangleSize);
            Point nextPoint = Reactangles.Count == 0 ? 
                spiralGenerator.GetNextPoint(reactangleCenter) :
                spiralGenerator.GetNextPoint(reactangleCenter);
            return nextPoint;
        }

        private Point ShiftPointRelativeTo(Point point, Point otherPoint) =>
            Point.Add(point, new Size(otherPoint));

        private Point GetCenterPointFor(Size rectangleSize) =>
            new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);

        public int GetWidth()
        {
            if (Reactangles.Count == 0)
                return 0;

            return Reactangles.Max(r => r.Right) - Reactangles.Min(r => r.Left);
        }

        public int GetHeight()
        {
            if (Reactangles.Count == 0)
                return 0;

            return Reactangles.Min(r => r.Bottom) - Reactangles.Max(r => r.Top);
        }
    }
}
