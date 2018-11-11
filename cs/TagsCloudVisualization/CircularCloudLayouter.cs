using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> usedRectangles;
        private int angle;
        private readonly double spiralLengthMultiplier;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            angle = -1;
            spiralLengthMultiplier = 1e-2;
            usedRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Width and height should be positive numbers");

            Rectangle currentRectangle;
            do
            {
                var currentLocation = GetNextPoint();
                currentRectangle = new Rectangle(currentLocation, rectangleSize);
            } while (usedRectangles.Any(rect => rect.IntersectsWith(currentRectangle)));
            usedRectangles.Add(currentRectangle);
            return currentRectangle;
        }

        private Point GetNextPoint()
        {
            angle++;
            var dx = (int)(Math.Cos(angle) * angle * spiralLengthMultiplier);
            var dy = (int)(Math.Sin(angle) * angle * spiralLengthMultiplier);
            return new Point(center.X + dx, center.Y + dy);
        }
    }
}
