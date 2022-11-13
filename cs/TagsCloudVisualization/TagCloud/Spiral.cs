using System;
using System.Drawing;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary>  Арифметическая спираль </summary>
    public class Spiral
    {
        private readonly Point center;
        private double angle;
        private readonly double delta;
        private readonly double density;

        public Spiral(Point center, double delta = 0.1, double density = 1.1)
        {
            this.center = center;
            this.delta = delta;
            this.density = density;
            this.angle = 0;
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
