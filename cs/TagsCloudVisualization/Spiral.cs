using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private double angleDelta = 0.1;
        private const double SpiralWidth = 0.4;
        private double currentAngle;
        public Point Center { get; }
        private int piFactor = 2;

        public Spiral(Point center)
        {
            Center = center;
        }

        public Point GetNextSpiralPosition()
        {
            var x = Center.X + (int)(SpiralWidth * currentAngle * Math.Cos(currentAngle));
            var y = Center.Y + (int)(SpiralWidth * currentAngle * Math.Sin(currentAngle));
            currentAngle += angleDelta;
            if (!(currentAngle > piFactor * Math.PI))
                return new Point(x, y);
            piFactor *= 2;
            angleDelta *= 0.7;
            return new Point(x, y);
        }
    }
}