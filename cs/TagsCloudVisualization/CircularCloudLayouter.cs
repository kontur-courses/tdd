using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : ILayouter
    {
        public Point Center { get; }

        private const double DoublePi = Math.PI * 2;
        private double startAngle = 0;
        private readonly double radiusThreshold = 0;
        private readonly double searchAngleStep = Math.PI / 2;
        private readonly double offsetAngleStep = Math.PI / 180;
        private readonly double minStep = 30d;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly Random random = new Random();

#if DEBUG
        public delegate void CircularLayoutState(
            List<Rectangle> rectangles,
            double radius,
            double angle,
            Rectangle currentRectangle);

        public event CircularLayoutState DebugStateChange;
#endif

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            var bestPoint = new Point(int.MaxValue, int.MaxValue);
            var bestDistance = double.PositiveInfinity;
            var radiusStep = LinearMath.GetDiagonal(size) / 2;
            var angle = startAngle;
            while (angle < startAngle + DoublePi)
            {
                var point = SearchBestOnAngle(angle, radiusStep, size);
                var pointDistance = LinearMath.DistanceBetween(Center, point.CenterWith(size));
                if (pointDistance < bestDistance)
                {
                    bestPoint = point;
                    bestDistance = LinearMath.DistanceBetween(Center, bestPoint.CenterWith(size));
                }

                angle += searchAngleStep;
            }

            startAngle += offsetAngleStep + (random.NextDouble() - 0.5) * 2;
            var rectangle = new Rectangle(bestPoint, size);
            rectangles.Add(rectangle);
            return rectangle;
        }

        public Point SearchBestOnAngle(double angle, double step, Size size)
        {
            var direction = -1;
            var radius = radiusThreshold;
            var rectangle = new Rectangle(LinearMath.PolarToCartesian(Center, radius, angle), size);
            
            while (!CanPlaced(rectangle))
                rectangle.Location = LinearMath.PolarToCartesian(Center, radius += step, angle); 
            var bestPoint = rectangle.Location;
            while (step > minStep)
            {
                rectangle.Location = LinearMath.PolarToCartesian(Center, radius += step * direction, angle);
                if (CanPlaced(rectangle))
                {
                    bestPoint = rectangle.Location;
                    direction = -1;
                }
                else
                    direction = 1;

                step /= 2;
#if DEBUG
                DebugStateChange?.Invoke(rectangles, radius, angle, rectangle);
#endif
            }

            return bestPoint;
        }

        private bool CanPlaced(Rectangle targetRectangle)
        {
            // TODO: Not best algorithm
            return rectangles.All(rectangle => !targetRectangle.IntersectsWith(rectangle));
        }
    }
}
