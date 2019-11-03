using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public readonly Point Center;
        public double angle { get; private set; }
        private readonly double angleShift;
        public double radius{ get; private set; }
        private readonly double radiusShift;
        private double densityCoefficient;

        public Spiral(Point center, double angleShift = 0.2, double radiusShift = 0.5, double densityCoefficient = 1)
        {
            Center = center;
            this.angleShift = angleShift;
            this.radiusShift = radiusShift;
            this.densityCoefficient = densityCoefficient;
        }

        public Point GetNextPoint()
        {
            radius += radiusShift / densityCoefficient;
            angle += angleShift;
            densityCoefficient += 0.01;

            return new Point((int) (Center.X + radius * Math.Cos(angle)),
                (int) (Center.Y + radius * Math.Sin(angle)));
        }
    }
}