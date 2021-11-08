using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Spiral
    {
        private double radius = 0;
        private double angle = 0;

        private Point center;

        private PointD currentPoint =>
            new PointD(
                Math.Cos(angle) * radius + center.X,
                Math.Sin(angle) * radius + center.Y);


        public Spiral(Point center)
        {
            this.center = center;
        }

        public void IncreaseSize(double radiusIncreaseValue = 0.1, double angleIncreaseValue = 0.1)
        {
            radius += radiusIncreaseValue;
            angle += angleIncreaseValue;
        }
    }
}