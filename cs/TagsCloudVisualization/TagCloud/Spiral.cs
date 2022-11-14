using System;
using System.Drawing;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary>  Арифметическая спираль </summary>
    public class Spiral : ISpiral
    {
        private readonly Point center;
        private double angle;
        private readonly double delta;
        private readonly double density;

        public Spiral(Point center, double delta = 0.1, double density = 1.1)
        {
            if (delta == 0 || density == 0)
            {
                throw new ArgumentException();
            }

            this.center = center;
            this.delta = delta;
            this.density = density;
            angle = 0;
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
            point.X += (int)Math.Round(radius * Math.Cos(angle));
            point.Y += (int)Math.Round(radius * Math.Sin(angle));
            angle += delta;

            return point;
        }
    }
}
