using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private double _a = 0;
        private double _b = 1;

        public ArchimedeanSpiral(double a = 0, double b = 1)
        {
            if (a < 0 || b <= 0)
                throw new ArgumentException("Parameters cannot be negative.");
            _a = a;
            _b = b;
        }

        public Point GetPoint(double angle)
        {
            double radius = _a + _b * angle;
            int x = Convert.ToInt32(radius * Math.Cos(angle));
            int y = Convert.ToInt32(radius * Math.Sin(angle));
            return new Point(x, y);
        }
    }
}