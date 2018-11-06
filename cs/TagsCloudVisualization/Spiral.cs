using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private const double AngleDelta = 0.1;
        private const double SpiralWidth = 1;
        private double currentAngle;
        public Point Center { get; } 

        public Spiral(Point center)
        {
            Center = center;
        }

        public Point GetNextSpiralPosition()
        {
            var x = Center.X + (int)(SpiralWidth * currentAngle * Math.Cos(currentAngle));
            var y = Center.Y + (int)(SpiralWidth * currentAngle * Math.Sin(currentAngle));
            currentAngle += AngleDelta;
            return new Point(x, y);
        }
    }
}