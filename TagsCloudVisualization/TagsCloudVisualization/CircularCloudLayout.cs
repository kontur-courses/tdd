using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        private List<Rectangle> rectangles;
        private const int turnFactor = 1;
        private const int distanceFactor = 5;
        private const double step = 0.1;
        private double angle = 0;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("Invalid center");
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Invalid size");
            var nextRectangle = new Rectangle(center, rectangleSize);
            while (rectangles.Any(rectangle => rectangle.IntersectsWith(nextRectangle)))
                nextRectangle.Location = GetNextRectangleLocation();
            rectangles.Add(nextRectangle);
            return new Rectangle(nextRectangle.Location, rectangleSize);
        }

        private Point GetNextRectangleLocation()
        {
            var t = Math.PI * angle;
            var x = (turnFactor + distanceFactor * t) * Math.Cos(t);
            var y = (turnFactor + distanceFactor * t) * Math.Sin(t);
            angle += step;
            if (x < 0 || y < 0)
                return new Point(center.X, center.Y);
            else
                return new Point(center.X + (int)x, center.Y + (int)y);
        }


    }
}
