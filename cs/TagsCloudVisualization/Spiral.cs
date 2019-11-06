using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private const double A = 0.5;
        private const double B = 0.5;
        private double phi = -Math.PI / 24;
        private double Radius => A + B * phi;

        private Point center;
        public Spiral(Point center)
        {
            this.center = center;
        }
        public Point GetNextPoint()
        {
            phi += Math.PI / 24;
            return new Point(center.X + (int)Math.Round(Radius * Math.Cos(phi)), center.Y + (int)Math.Round(Radius * Math.Sin(phi)));
        }

    }
}