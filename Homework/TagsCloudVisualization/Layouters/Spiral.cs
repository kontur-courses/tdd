using System;
using System.Drawing;
using TagsCloudVisualization.Helpers;

namespace TagsCloudVisualization.Layouters
{
    public class Spiral
    {
        private double radius = 0;
        private double angle = 0;

        private readonly Point center;

        public PointD CurrentPoint =>
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