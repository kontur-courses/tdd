using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        public Point StartPoint { get; }
        public double StartRadius { get; }
        public double ExtendRatio { get;  }

        public ArchimedeanSpiral(Point startPoint, double startRadius = 0, double extendRatio = 1)
        {
            StartPoint = startPoint;
            if (startRadius < 0 || extendRatio <= 0)
                throw new ArgumentException("Parameters cannot be negative.");
            StartRadius = startRadius;
            ExtendRatio = extendRatio;
        }

        public Point GetPoint(double angle)
        {
            double radius = StartRadius + ExtendRatio * angle;
            int x = StartPoint.X + Convert.ToInt32(radius * Math.Cos(angle));
            int y = StartPoint.Y + Convert.ToInt32(radius * Math.Sin(angle));
            return new Point(x, y);
        }
    }
}