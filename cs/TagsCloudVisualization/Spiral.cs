using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Класс реализует  спираль Архимеда
    /// </summary>
    public class Spiral
    {
        private const double TwoPi = 2 * Math.PI;

        private const double Step = 0.1;

        private int nextAngle;

        private Point center;

        public Spiral(Point center)
        {
            this.center = center;
            nextAngle = 0;
        }

        public Point CalculatePointSpiral()
        {
            var radius = Step * nextAngle / TwoPi;
            var x = (int)(radius * Math.Cos(nextAngle) + center.X);
            var y = (int)(radius * Math.Sin(nextAngle) + center.Y);

            nextAngle++;

            return new Point(x, y);
        }
    }
}