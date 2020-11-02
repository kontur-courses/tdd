using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point _center;
        private List<Rectangle> rectangles;
        private int cloudRadius;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException();
            _center = center;
            rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
            var radius = Math.Max((double)cloudRadius-8, 0);
            var angle = 0d;
            while (true)
            {
                var coord = new Point(
                    (int)(_center.X + radius * Math.Cos(angle) - rectangleSize.Width / 2),
                    (int)(_center.Y - radius * Math.Sin(angle)) - rectangleSize.Height / 2);
                var availableToPlace = true;
                var currentRectangle = new Rectangle(coord, rectangleSize);
                foreach (var rectangle in rectangles)
                    if (rectangle.IntersectsWith(currentRectangle))
                    {
                        availableToPlace = false;
                        break;
                    }

                if (availableToPlace)
                {
                    cloudRadius = Math.Max(currentRectangle.Right - _center.X, cloudRadius);
                    rectangles.Add(currentRectangle);
                    break;
                }

                radius += 0.02;
                angle += 0.3;
            }

            return rectangles.Last();
        }

        public List<Rectangle> GetRectangles() => rectangles;
    }
}
