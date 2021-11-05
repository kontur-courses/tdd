using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private readonly Point center;
        private readonly int radius;

        public ArchimedeanSpiral(Point center, int radius)
        {
            if (radius <= 0)
                throw new ArgumentException("Radius should be positive.", nameof(radius));

            this.center = center;
            this.radius = radius;
        }

        public Point GetPoint(int degree)
        {
            if (degree < 0)
                throw new ArgumentException("Degree can't be negative.", nameof(degree));

            var angle = degree * Math.PI / 180;
            var length = radius * angle;
            return new Point
            {
                X = (int)(length * Math.Cos(angle)) + center.X,
                Y = -(int)(length * Math.Sin(angle)) + center.Y
            };
        }
    }
}