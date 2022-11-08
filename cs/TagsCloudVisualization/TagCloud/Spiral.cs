using System;
using System.Drawing;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary>  Арифметическая спираль </summary>
    public class Spiral
    {
        private const double delta = Math.PI / 360;
        private const double density = 1.1;
        private const double defaultAngle = 0;

        private readonly Point center;
        private double angle;

        public Spiral(Point center)
        {
            this.center = center;
            this.angle = defaultAngle;
        }

        public Point GetNextPoint()
        {
            var radius = density * angle;

            var point = new Point(center.X, center.Y);
            point.X += (int)(radius * Math.Cos(angle));
            point.Y += (int)(radius * Math.Sin(angle));
            angle += delta;

            return point;
        }
    }
}
