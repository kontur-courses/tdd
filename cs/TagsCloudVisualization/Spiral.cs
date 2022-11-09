using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private const double AngleOffset = 1;
        private const double RadiusOffset = 0.1;
        private Point center;
        private List<Point> usedPoints;
        private double angle = 0;
        private double radius = 0;

        public Spiral(Point center)
        {
            this.center = center;
            usedPoints = new List<Point>();
        }

        public IEnumerable<Point> GetPoints()
        {
            if (angle == 0 && radius == 0)
                yield return center;
            while (true)
            {
                var newPoint = new Point(center.X + (int) Math.Round(radius * Math.Cos(angle)),
                    center.Y + (int) Math.Round(radius * Math.Sin(angle)));
                radius += RadiusOffset;
                angle += AngleOffset;
                if (usedPoints.Contains(newPoint))
                    continue;
                usedPoints.Add(newPoint);
                yield return newPoint;
            }
        }
    }
}