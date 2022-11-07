using System;
using System.Drawing;

namespace TagCloud
{
    public class ArchimedeanSpiral
    {
        public readonly Point _centralPoint;

        public double Parameter { get; set; }

        public double Angle { get; set; } = 0;

        private readonly double _deltaAngle = 2 * Math.PI / 360;

        public ArchimedeanSpiral(Point centralPoint, double parameter = 1)
        {
            _centralPoint = centralPoint;
            Parameter = parameter;
        }

        public Point GetNextPoint()
        {
            Point nextPoint = new Point((int)(_centralPoint.X + Parameter * Angle * Math.Cos(Angle)),
                (int)(_centralPoint.Y + Parameter * Angle * Math.Sin(Angle)));

            Angle += _deltaAngle;

            return nextPoint;
        }
    }
}
