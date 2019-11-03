using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException(
                    "Height and width of rectangle size must be greater than zero!");

            /* get points on spiral until we find a point which we can put the rectangle on
            * so it's not intersecting with any others already added to layout */
            var resultRectangle = new Rectangle(center, rectangleSize);
            while (IsIntersectingWithLayout(resultRectangle = 
                new Rectangle(GetNextSpiralPoint(), rectangleSize)));

            resultRectangle = TryGetRectangleShiftedCloserToCenter(resultRectangle);
            rectangles.Add(resultRectangle);
            return resultRectangle;
        }

        private bool IsIntersectingWithLayout(Rectangle rectangle)
        {
            return rectangles.Any(rect => rect.IntersectsWith(rectangle));
        }

        private Rectangle TryGetRectangleShiftedCloserToCenter(Rectangle rectangle, 
            int attempts = 5)
        {
            if (GetRadiusLength(rectangle.Location) < spiralStepLength)
                return rectangle;
            var deltaLength = spiralStepLength / (attempts + 1.0);
            var angle = Math.Acos(
                (rectangle.Location.X - center.X) / GetRadiusLength(rectangle.Location));
            var deltaShift = new Point(
                -(int)(deltaLength*Math.Cos(angle)), 
                -(int)(deltaLength*Math.Sin(angle)));
            do
            {
                rectangle.Offset(deltaShift);
                attempts--;
            }
            while (!IsIntersectingWithLayout(rectangle) && attempts >= 0);
            rectangle.Offset(-deltaShift.X, -deltaShift.Y);

            return rectangle;
        }

        private double GetRadiusLength(Point point)
        {
            point.Offset(-center.X, -center.Y);
            return Math.Sqrt(point.X*point.X + point.Y*point.Y);
        }

        // Archimedean spiral
        private readonly double spiralStepLength = 10;
        private readonly int amountOfSubsteps = 36;
        private double currentSpiralAngle = 0;

        private Point GetNextSpiralPoint()
        {
            var spiralAngleDelta = 2 * Math.PI / amountOfSubsteps;
            var angle = currentSpiralAngle;
            var radiusLength = spiralStepLength * angle / (2 * Math.PI);
            currentSpiralAngle += spiralAngleDelta;
            return new Point(
                    (int)(radiusLength * Math.Cos(angle) + center.X),
                    (int)(radiusLength * Math.Sin(angle) + center.Y));
        }
    }
}
