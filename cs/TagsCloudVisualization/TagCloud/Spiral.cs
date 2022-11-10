using System;
using System.Drawing;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary>  Арифметическая спираль </summary>
    public class Spiral
    {
        private const double defaultDelta = 0.1;
        private const double defaultDensity = 1.1;
        private const double defaultAngle = 0;

        private readonly Point center;
        private double angle;
        private double delta;
        private double density;

        public Spiral(Point center, double delta = defaultDelta, double density = defaultDensity)
        {
            this.center = center;
            this.delta = delta;
            this.density = density;
            this.angle = defaultAngle;
        }

        public Point GetNextPoint()
        {
            var point = new Point(center.X, center.Y);

            if (angle == 0)
            {
                angle += delta;
                return point;
            }

            var radius = density * angle;
            point.X += (int)(radius * Math.Cos(angle));
            point.Y += (int)(radius * Math.Sin(angle));
            angle += delta;

            return point;
        }
    }
}
