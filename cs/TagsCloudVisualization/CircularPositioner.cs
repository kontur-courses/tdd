using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularPositioner
    {
        private const double DoublePi = Math.PI * 2;
        private const double MinStep = 1d;

        private readonly Point center;
        private readonly double iterationOffset;
        private readonly double radiusThreshold;
        private readonly Random random = new Random();
        private readonly double searchAngleStep;
        private double startAngle;

        public CircularPositioner(
            Point center,
            double radiusThreshold,
            double searchAngleStep,
            double iterationOffset)
        {
            this.center = center;
            this.radiusThreshold = radiusThreshold;
            this.searchAngleStep = searchAngleStep;
            this.iterationOffset = iterationOffset;
        }

        public double RadiusStep { get; set; } = 20;

        public IEnumerable<Point> Iteration(Func<Point, bool> isValid)
        {
            var angle = startAngle;
            while (angle < startAngle + DoublePi)
            {
                var point = SearchBestPlaceOnAngle(angle, isValid);
                yield return point;

                angle += searchAngleStep;
            }

            startAngle += iterationOffset + (random.NextDouble() - 0.5) * 2;
        }

        private Point SearchBestPlaceOnAngle(double angle, Func<Point, bool> isValid)
        {
            var direction = -1;
            var radius = radiusThreshold;
            var point = LinearMath.PolarToCartesian(center, radius, angle);
            var step = RadiusStep;

            while (!isValid(point))
                point = LinearMath.PolarToCartesian(center, radius += step, angle);
            var bestPoint = point;
            while (step > MinStep)
            {
                point = LinearMath.PolarToCartesian(center, radius += step * direction, angle);
                if (isValid(point))
                {
                    bestPoint = point;
                    direction = -1;
                }
                else
                {
                    direction = 1;
                }

                step /= 2;
            }

            return bestPoint;
        }
    }
}
