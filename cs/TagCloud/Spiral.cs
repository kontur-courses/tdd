using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagCloud
{
    public class Spiral
    {
        private Point center;
        private double rayLength;
        private double angle;

        public Spiral(Point center)
        {
            this.center = center;
            rayLength = 0;
            angle = -1 * Math.PI / 180;
        }

        public Point GetNextPoint()
        {
            angle += Math.PI / 180;
            rayLength = 1d / (2 * Math.PI) * angle;
            return new Point((int)(center.X + rayLength * Math.Cos(angle)), (int)(center.Y + rayLength * Math.Sin(angle)));
        }
    }
}