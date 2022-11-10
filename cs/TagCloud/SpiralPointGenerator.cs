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

        private Size center;

        public SpiralPointGenerator(Point center)
        {
            this.center = new Size(center);
        }

        public Point GetNextPoint(Size currentRectangleCenter)
        {
            var point = GetNextPoint();
            point = Shift(point, inDirection: center);
            point = Shift(point, inDirection: currentRectangleCenter);
            return point;
        }

        private Point Shift(Point point, Size inDirection) =>
            Point.Add(point, inDirection);
        

        private Point GetNextPoint()
        {
            var point = new Point(
                x: (int)(radius * Math.Cos(theta)),
                y: (int)(radius * Math.Sin(theta)));

            DoStep();

            return point;
        }

        private void DoStep()
        {
            theta += thetaStep;
            radius += radiusStep;
        }
    }
}