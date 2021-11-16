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
        private readonly HashSet<Point> usedPoints = new();


        private float currentStepAngle = MathF.PI / 2;
        private float currentRadius;
        private float radiusStep = 10;
        private int currentStep = 1;
        private int limiter = 100000;
        private int countSteps = 1;


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = GetRectangleLocation(rectangleSize);
            var nextRectangle = new Rectangle(location, rectangleSize);

            rectangles.Add(nextRectangle);
            usedPoints.UnionWith(GeometryHelper.GetAllPointIntoRectangle(nextRectangle));

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
                    angle = currentStep * currentStepAngle;
                    currentStep++;
                    
                    if (usedPoints.Contains(rectangleLocation)) continue;

                    var rectangle = new Rectangle(rectangleLocation, rectangleSize);


                    if (!rectangles.Any(r => r.IntersectsWith(rectangle)))
                    {
                        currentStep = 1;
                        currentRadius = 1;
                        currentStepAngle = MathF.PI / 2;
                        return rectangle.Location;
                    }
                }

                UpdateParameters();
            }

            throw new Exception();
        }


        private void UpdateParameters()
        {
            currentRadius += radiusStep;
            if (countSteps % 4 == 0)
                currentStepAngle /= 2;
            countSteps++;
            currentStep = 1;
        }


        public IReadOnlyList<Rectangle> GetRectangles()
            => rectangles;
    }
}