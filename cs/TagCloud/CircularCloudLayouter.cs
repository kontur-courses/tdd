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

        private double theta = 0;
        private double radius = 0;
        private readonly double thetaStep = Math.PI / 360;
        private readonly double radiusStep = 0.01;

        public CircularCloudLayouter() : this(new Point())
        {
        }

        public CircularCloudLayouter(Point center)
        {
            Center = new Point(center.X, center.Y);
            Reactangles = new List<Rectangle>();
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
                GetNextPoint(reactangleCenter) :
                GetNextPoint(reactangleCenter);
            return nextPoint;
        }

        private Point ShiftPointRelativeTo(Point point, Point otherPoint) =>
            Point.Add(point, new Size(otherPoint));

        private Point GetCenterPointFor(Size rectangleSize) =>
            new Point(-rectangleSize.Width / 2, -rectangleSize.Height / 2);

        public Point GetNextPoint(Point currentRectangleCenter)
        {
            var point = new Point(
                x: Center.X + (int)(radius * Math.Cos(theta)),
                y: Center.Y + (int)(radius * Math.Sin(theta)));

            theta += thetaStep;
            radius += radiusStep;
            return Point.Add(point, new Size(currentRectangleCenter));
        }

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
