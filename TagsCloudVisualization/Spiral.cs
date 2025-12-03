using System;
using System.Drawing;


namespace TagCloud
{
    public class Spiral : IPointGetter
    {
        private readonly Point center;
        private readonly double angleStep;
        private readonly double radiusStep;
        private double angle;


        public Spiral(Point center, double angleStep = 0.1, double radiusStep = 0.5)
        {
            this.center = center;
            this.angleStep = angleStep;
            this.radiusStep = radiusStep;
            angle = 0;
        }


        public Point GetNextPoint()
        {
            double radius = radiusStep * angle;
            var x = center.X + radius * Math.Cos(angle);
            var y = center.Y + radius * Math.Sin(angle);
            angle += angleStep;
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }


    }
}