using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point Center;
        private readonly double AngleShift;
        private readonly double RadiusShift;
        public double Radius { get; private set; }
        public double Angle{ get; private set; }
        private double DensityCoefficient;

        public Spiral(Point center, double angleShift = 0.2, double radiusShift = 0.5, double densityCoefficient = 1)
        {
            Center = center;
            AngleShift = angleShift;
            RadiusShift = radiusShift;
            DensityCoefficient = densityCoefficient;
        }

        public Point GetNextPoint()
        {
            Radius += RadiusShift / DensityCoefficient;
            Angle += AngleShift;
            DensityCoefficient += 0.01;

            return new Point((int) (Center.X + Radius * Math.Cos(Angle)),
                (int) (Center.Y + Radius * Math.Sin(Angle)));
        }
    }
}