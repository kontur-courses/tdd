using System;
using System.Drawing;

namespace TagCloud
{
    public class SpiralPointGenerator
    {
        private double theta = 0;
        private double radius = 0;
        private readonly double thetaStep = Math.PI / 360;
        private readonly double radiusStep = 0.01;
        private Point center;

        public SpiralPointGenerator(Point center)
        {
            this.center = center;
        }

        public Point GetNextPoint(Point currentRectangleCenter)
        {
            var point = new Point(
                x: center.X + (int)(radius * Math.Cos(theta)),
                y: center.Y + (int)(radius * Math.Sin(theta)));
            
            theta += thetaStep;
            radius += radiusStep;
            return Point.Add(point, new Size(currentRectangleCenter));
        }
    }
}