using System;
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
            var xCoord = (int) (center.X + rayLength * Math.Cos(angle));
            var yCoord = (int) (center.Y + rayLength * Math.Sin(angle));
            return new Point(xCoord, yCoord);
        }
    }
}