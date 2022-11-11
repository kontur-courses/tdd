using System;
using System.Drawing;

namespace TagCloud
{
    public class ArchimedeanSpiral
    {
        private const double DeltaAngle = 2 * Math.PI / 360;

        public readonly Point CentralPoint;

        private readonly double scaleFactor;

        private double angle;

        public ArchimedeanSpiral(Point centralPoint, double scaleFactor = 1)
        {
            if (scaleFactor <= 0)
                throw new ArgumentException("scale factor must be more than zero");

            CentralPoint = centralPoint;
            this.scaleFactor = scaleFactor;
        }

        public Point GetNextPoint()
        {
            var currentAngle = GetAngleAndUpdate();

            var x = CentralPoint.X + (int)(scaleFactor * currentAngle * Math.Cos(currentAngle));

            var y = CentralPoint.Y + (int)(scaleFactor * currentAngle * Math.Sin(currentAngle));

            return new Point(x, y);
        }

        private double GetAngleAndUpdate()
        {
            var result = angle;

            angle += DeltaAngle;

            return result;
        }

    }
}
