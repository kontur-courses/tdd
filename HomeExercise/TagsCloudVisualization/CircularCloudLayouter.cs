using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point _center;
        public readonly List<FilledArea> FilledAreas = new List<FilledArea>();

        public CircularCloudLayouter(Point center) => _center = center;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException("Size values must non-negative");
            Point rectanglePosition;
            if (FilledAreas.Count == 0)
                rectanglePosition = CenterPosition(_center, rectangleSize);
            else
            {
                var furtherPoint = FilledAreas.Last().BottomRight;
                rectanglePosition = new Point(furtherPoint.X + 1, furtherPoint.Y + 1);
            }
            
            var newRectangle = new Rectangle(rectanglePosition, rectangleSize);
            FilledAreas.Add(new FilledArea(newRectangle));
            return newRectangle;
        }

        private static Point CenterPosition(Point center, Size rectangleSize)
        {
            var xPosition = center.X - rectangleSize.Width / 2;
            var yPosition = center.Y + rectangleSize.Height / 2;
            return new Point(xPosition, yPosition);
        }
    }
}