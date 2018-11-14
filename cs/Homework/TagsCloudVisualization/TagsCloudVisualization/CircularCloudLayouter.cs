using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly SpiralInfo spiralInfo;
        private HashSet<Rectangle> rectangles;
        public HashSet<Rectangle> Rectangles => new HashSet<Rectangle>(rectangles);

        public CircularCloudLayouter(Point center, double radiusStep = 0.0001, double angleStep = 0.1)
        {
            rectangles = new HashSet<Rectangle>();
            spiralInfo = new SpiralInfo(radiusStep, angleStep, center);
        }

        public CircularCloudLayouter(SpiralInfo spiralInfo)
        {
            this.spiralInfo = spiralInfo;
        }

        public Rectangle PutNextRectangle(Size rectangleSize, Point? position = null)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height should be integer positive numbers");

            var newRectangle = default(Rectangle);

            if (position == null)
                newRectangle = new Rectangle(GetAppropriatePlace(rectangleSize), rectangleSize);
            else
                newRectangle = new Rectangle((Point)position, rectangleSize);

            rectangles.Add(newRectangle);

            return newRectangle;
        }

        public void ReplaceRectangles()
        {
            var rectanglesCopy = new HashSet<Rectangle>(rectangles);
            rectangles.Clear();

            foreach (var rectangle in rectanglesCopy
                .OrderBy(rectangle => rectangle.Height * rectangle.Width))
            {
                rectangles.Add(new Rectangle(GetAppropriatePlace(rectangle.Size), rectangle.Size));
            }
        }

        public Point GetAppropriatePlace(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return spiralInfo.Center;

            var currentPosition = spiralInfo.Center;
            var currentAngle = 0.0;
            var currentRadius = 0.0;
            var potentialRectangle = new Rectangle(currentPosition, rectangleSize);

            while (!CanBePlaced(potentialRectangle))
            {
                (currentPosition, currentAngle, currentRadius) = NextStep(
                    currentPosition, currentAngle, currentRadius);
                if (!potentialRectangle.Location.Equals(currentPosition))
                    potentialRectangle = new Rectangle(currentPosition, rectangleSize);
            }
            return currentPosition;
        }

        private ValueTuple<Point, double, double> NextStep(
            Point currentPosition, double currentAngle, double currentRadius)
        {
            var sin = Math.Sin(currentAngle);
            var cos = Math.Cos(currentAngle);
            currentPosition.X = (int)(spiralInfo.Center.X - currentRadius * currentAngle * sin);
            currentPosition.Y = (int)(spiralInfo.Center.Y + currentRadius * currentAngle * cos);
            currentAngle = (currentAngle + spiralInfo.AngleStep) % SpiralInfo.MaxAngle;
            currentRadius += spiralInfo.RadiusStep;

            return (currentPosition, currentAngle, currentRadius);
        }

        private bool CanBePlaced(Rectangle rectangle)
        {
            return !HaveIntersectionWithAnotherRectangle(rectangle) &&
                   !IsPlacedInAnotherRectangle(rectangle);
        }

        private bool HaveIntersectionWithAnotherRectangle(Rectangle rectangle)
        {
            return rectangles.Any(anotherRectangle => RectanglesChecker.HaveIntersection(rectangle, anotherRectangle));
        }

        private bool IsPlacedInAnotherRectangle(Rectangle rectangle)
        {
            return rectangles.Any(anotherRectangle => RectanglesChecker.IsNestedRectangle(rectangle, anotherRectangle) &&
                                                      RectanglesChecker.IsNestedRectangle(anotherRectangle, rectangle));
        }
    }
}
