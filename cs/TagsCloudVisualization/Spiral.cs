using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private Point? _lastPoint;
        public Point Center { get; }
        public double Phi { get; private set; }
        public double SpiralParam { get; }
        public double DeltaPhi { get; }

        public Spiral(Point center, double spiralParam = 1, double deltaPhi = Math.PI / 180)
        {
            ValidateParams(spiralParam);

            Center = center;
            Phi = 0;
            SpiralParam = spiralParam;
            DeltaPhi = deltaPhi;
        }

        public Point GetNextPoint()
        {
            Point newPoint;

            do
            {
                var x = Math.Round(SpiralParam * Phi * Math.Cos(Phi)) + Center.X;
                var y = Math.Round(SpiralParam * Phi * Math.Sin(Phi)) + Center.Y;
                Phi += DeltaPhi;

                newPoint = new Point((int)x, (int)y);
            } while (newPoint == _lastPoint);

            _lastPoint = newPoint;
            return newPoint;
        }

        private static void ValidateParams(double spiralParam)
        {
            if (spiralParam <= 0)
                throw new ArgumentException("Spiral param must be great than zero");
        }
    }
}