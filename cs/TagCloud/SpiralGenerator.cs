using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;

namespace TagCloud
{
    public class SpiralGenerator
    {
        private double theta = 0;
        private readonly double thetaStep = Math.PI / 60;
        private readonly double radiusStep = 30;
        private Point center;

        public SpiralGenerator(Point center)
        {
            this.center = center;
        }

        public Point GetNextPoint(Rectangle? previousRectangle)
        {
            if (previousRectangle is null)
                return center;

            double delta = Math.Pow(Math.Pow(previousRectangle.Value.Width, 2) +
                                    Math.Pow(previousRectangle.Value.Height, 2), 0.5);
            theta += thetaStep;
            delta += Math.Ceiling(theta / (2 * Math.PI)) * radiusStep;
            var point = new Point(
                x: (int) (center.X + delta * Math.Cos(theta)),
                y: (int) (center.Y + delta * Math.Sin(theta)));
            return point;
        }
    }
}
