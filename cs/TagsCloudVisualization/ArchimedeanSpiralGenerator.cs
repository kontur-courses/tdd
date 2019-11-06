using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class ArchimedeanSpiralGenerator : CoordinateGenerator
    {
        private const double TwoPi = 2 * Math.PI;

        private readonly double distance;
        private readonly double delta;
        private double azimuth;
        private double radius;

        internal ArchimedeanSpiralGenerator(double distance, double delta = Math.PI / 180)
        {
            this.distance = distance;
            this.delta = delta;
        }

        public IEnumerable<Point> GeneratePoints()
        {
            while (radius <= int.MaxValue)
            {
                yield return CreatePointFromPolar(radius, azimuth);
                azimuth += delta;
                radius = distance * azimuth / TwoPi;
            }
        }

        private static Point CreatePointFromPolar(double r, double phi) =>
            new Point((int)(r * Math.Cos(phi)), (int)(r * Math.Sin(phi)));
    }
}
