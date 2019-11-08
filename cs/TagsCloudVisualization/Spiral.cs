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
        public double Angle { get; private set; }
        private double DensityCoefficient;
        private double DensityShift;

        public enum SpiralDensity
        {
            verySparse,
            sparse,
            dense,
            veryDense
        }

        public Spiral(Point center, SpiralDensity densityType = SpiralDensity.dense, double angleShift = 0.2,
            double radiusShift = 0.5)
        {
            Center = center;
            AngleShift = angleShift;
            RadiusShift = radiusShift;
            SetUpDensity(densityType);
        }

        private void SetUpDensity(SpiralDensity density)
        {
            switch (density)
            {
                case SpiralDensity.dense:
                    DensityCoefficient = 1;
                    DensityShift = 0.01;
                    break;
                case SpiralDensity.veryDense:
                    DensityCoefficient = 3;
                    DensityShift = 0.02;
                    break;
                case SpiralDensity.sparse:
                    DensityCoefficient = 0.8;
                    DensityShift = 0;
                    break;
                case SpiralDensity.verySparse:
                    DensityCoefficient = 0.5;
                    DensityShift = 0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(density), density, null);
            }
        }

        public Point GetNextPoint()
        {
            Radius += RadiusShift / DensityCoefficient;
            Angle += AngleShift;
            DensityCoefficient += DensityShift;

            return new Point((int) (Center.X + Radius * Math.Cos(Angle)),
                (int) (Center.Y + Radius * Math.Sin(Angle)));
        }
    }
}