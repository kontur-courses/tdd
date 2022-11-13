using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly double angleOffset;
        private readonly double radiusOffset;
        private readonly Point center;
        private readonly Dictionary<(double, double), Point> usedPoints;
        private double angle;
        private double radius;

        public Spiral(Point center, double angleOffset, double radiusOffset)
        {
            this.center = center;
            usedPoints = new Dictionary<(double, double), Point>();
            this.angleOffset = angleOffset;
            this.radiusOffset = radiusOffset;
        }

        public IEnumerable<Point> GetPoints()
        {
            (double, double) currentAngleAndRadius;
            if (angle == 0 && radius == 0)
            {
                currentAngleAndRadius = (0, 0);
                if (!usedPoints.ContainsKey(currentAngleAndRadius))
                    usedPoints.Add(currentAngleAndRadius, center);
                yield return usedPoints[currentAngleAndRadius];
            }

            while (true)
            {
                radius += radiusOffset;
                angle += angleOffset;
                currentAngleAndRadius = (angle, radius);
                if (usedPoints.ContainsKey(currentAngleAndRadius))
                    yield return usedPoints[currentAngleAndRadius];
                else
                {
                    var x = center.X + (int) Math.Round(radius * Math.Cos(angle));
                    var y = center.Y + (int) Math.Round(radius * Math.Sin(angle));
                    usedPoints.Add(currentAngleAndRadius, new Point(x, y));
                    yield return usedPoints[currentAngleAndRadius];
                }
            }
        }
    }
}