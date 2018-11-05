using System;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly double step;
        public double Angle { get; private set; }

        public Spiral(double step, double angle)
        {
            this.step = step;
            Angle = angle;
        }

        public void IncreaseAngle() => Angle++;

        public Point GetNextPoint(Point center)
        {
            var radius = step * Angle;
            var x = center.X + (int)(radius * Math.Cos(Angle));
            var y = center.Y + (int) (radius * Math.Sin(Angle));
            IncreaseAngle();
            return new Point(x, y);
        }
    }
}
