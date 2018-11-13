using System;
using System.Drawing;

namespace TagsCloudVisualization.Geom
{
    public class Spiral
    {
        private const double ThetaDelta = 0.01;

        public readonly Point Center;
        private double theta;

        public Spiral(Point center)
        {
            Center = center;
        }

        public PointF GetNextLocation()
        {
            var x = (float) (theta * Math.Cos(theta) + Center.X);
            var y = (float) (theta * Math.Sin(theta) + Center.Y);

            theta += ThetaDelta;

            return new PointF(x, y);
        }
    }
}