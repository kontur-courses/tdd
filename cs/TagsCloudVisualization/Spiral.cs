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
        private readonly List<Point> usedPoints;
        private double angle;
        private double radius;

        public Spiral(Point center, double angleOffset, double radiusOffset)
        {
            this.center = center;
            usedPoints = new List<Point>();
            this.angleOffset = angleOffset;
            this.radiusOffset = radiusOffset;
        }

        public IEnumerable<Point> GetPoints()
        {
            foreach (var point in usedPoints)
                yield return point;

            if (angle == 0 && radius == 0)
            {
                if (!usedPoints.Contains(center))
                    usedPoints.Add(center);
                yield return center;
            }

            while (true)
            {
                radius += radiusOffset;
                angle += angleOffset;
                var x = center.X + (int) Math.Round(radius * Math.Cos(angle));
                var y = center.Y + (int) Math.Round(radius * Math.Sin(angle));
                usedPoints.Add(new Point(x, y));
                yield return new Point(x, y);
            }
        }
    }
}