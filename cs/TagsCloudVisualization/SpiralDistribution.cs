using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class SpiralDistribution : IDistribution
    {
        public SpiralDistribution(Point center = new Point(), double angleStep = 0.1, double radiusStep = 0.1)
        {
            if (radiusStep <= 0 || angleStep == 0) throw new ArgumentException();
            Center = center;
            Radius = 0;
            Angle = 0;
            AngleStep = angleStep - 2 * Math.PI * (int)(angleStep / 2 * Math.PI);
            RadiusStep = radiusStep;
        }

        public double Angle { get; private set; }
        public double AngleStep { get; }
        public double RadiusStep { get; }
        public Point Center { get; }
        public double Radius { get; private set; }

        public Point GetNextPoint()
        {
            var x = Radius * Math.Cos(Angle) + Center.X;
            var y = Radius * Math.Sin(Angle) + Center.Y;

            Angle += AngleStep;

            if (Angle >= Math.PI * 2)
            {
                Angle -= 2 * Math.PI;
                Radius += RadiusStep;
            }

            return new Point((int)x, (int)y);
        }
    }
}