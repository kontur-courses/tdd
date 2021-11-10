using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }


        private const float Pi2 = 2 * MathF.PI;

        private readonly List<Rectangle> rectangles = new();

        private float currentStepAngle = MathF.PI / 2;
        private float currentRadius;
        private float radiusStep = 1;
        private int currentStep = 1;
        private int limiter = 100;


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = GetRectangleLocation(rectangleSize);
            var nextRectangle = new Rectangle(location, rectangleSize);

            rectangles.Add(nextRectangle);

            return nextRectangle;
        }


        private Point GetRectangleLocation(Size rectangleSize)
        {
            if (rectangles.Count != 0) return SearchPointOnCircle(rectangleSize);
            
            currentRadius = Math.Min(rectangleSize.Height, rectangleSize.Width);
            return GeometryHelper
                .GetRectangleLocationFromCentre(Center, rectangleSize);

        }

        private Point SearchPointOnCircle(Size rectangleSize)
        {
            while (currentRadius < limiter)
            {
                var angle = currentStep * currentStepAngle;

                while (angle < Pi2)
                {
                    var rectangleCentre =
                        GeometryHelper.GetPointOnCircle(Center, currentRadius, angle);
                    var rectangleLocation =
                        GeometryHelper.GetRectangleLocationFromCentre(rectangleCentre, rectangleSize);

                    var rectangle = new Rectangle(rectangleLocation, rectangleSize);

                    angle = currentStep * currentStepAngle;
                    currentStep++;

                    if (!rectangles.Any(r => r.IntersectsWith(rectangle))) return rectangle.Location;
                }

                UpdateParameters();
            }

            throw new Exception();
        }

        
        private void UpdateParameters()
        {
            currentRadius += radiusStep;
            currentStepAngle /= 2;
            currentStep = 1;
        }


        public IReadOnlyList<Rectangle> GetRectangles()
            => rectangles;
    }
}