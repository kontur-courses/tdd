using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        private readonly double densityParam;
        private readonly double angleStep;
        private readonly HashSet<Point> points;
        private double angle;

        private double Radius => angle * densityParam;

        public Spiral(Point center, double densityParam = 0.5, double angleStep = Math.PI / 180)
        {
            if (densityParam <= 0)
            {
                throw new ArgumentException("Density parameter must be a positive number");
            }

            this.center = center;
            this.densityParam = densityParam;
            this.angleStep = angleStep;
            points = new HashSet<Point>();
        }

        public Point GetNextPoint()
        {
            var nextPoint = CalculateNextPoint();
            while (points.Contains(nextPoint))
            {
                nextPoint = CalculateNextPoint();
            }

            points.Add(nextPoint);
            return nextPoint;
        }

        private Point CalculateNextPoint()
        {
            var x = (int)Math.Round(Radius * Math.Cos(angle));
            var y = (int)Math.Round(Radius * Math.Sin(angle));

            angle += angleStep;
            return new Point(center.X + x, center.Y + y);
        }
    }
}