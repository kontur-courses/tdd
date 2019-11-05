using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public double Step { get; set; }
        private double angle;
        private readonly double angleDelta;
        private readonly Point center;

        public Spiral(double angleDelta, Point center)
        {
            this.angleDelta = angleDelta;
            this.center = center;
        }

        public Point GetNextPoint()
        {
            var x = Convert.ToInt32(Step / (Math.PI * 2) * angle * Math.Cos(angle));
            var y = Convert.ToInt32(Step / (Math.PI * 2) * angle * Math.Sin(angle));
            angle += angleDelta;
            x += center.X;
            y += center.Y;
            return new Point(x, y);
        }
    }
}