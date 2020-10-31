using System;
using System.Collections.Generic;
using System.Drawing;

namespace CloudTag
{
    public class Spiral
    {
        private readonly Point center;
        private double angle;
        private readonly double radius;

        public Spiral(Point center, double radius = 1)
        {
            this.center = center;
            this.radius = radius;
        }

        public Point GetNextPoint()
        {
            var x = (int) (angle * Math.Cos(angle) * radius + center.X);
            var y = (int) (angle * Math.Sin(angle) * radius + center.Y);
            angle += Math.PI / 180;
            return new Point(x, y);
        }
    }
}