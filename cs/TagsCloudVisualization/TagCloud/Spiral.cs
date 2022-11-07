using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.TagCloud
{
    /// <summary>
    /// Арифметическая спираль
    /// </summary>
    public class Spiral
    {
        private const double defaultDelta = Math.PI / 360;
        private const double defaultDensity = 1.7;
        private const double defaultAngle = 0;

        private readonly Point center;
        private double angle;
        private readonly double density;
        private readonly double delta;

        public Spiral(Point center, double angle = defaultAngle, double density = defaultDensity, double delta = defaultDelta)
        {
            this.center = center;
            this.angle = angle;
            this.density = density;
            this.delta = delta;
        }

        public Point GetNewPoint()
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
