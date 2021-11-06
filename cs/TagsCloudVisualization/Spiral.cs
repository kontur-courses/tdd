using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Класс реалзует спираль Архимеда
    /// </summary>
    public class Spiral
    {
        private const double TwoPi = 2 * Math.PI;

        private const double Step = 0.5;

        private int countPoints;

        private Point center;

        public Spiral(Point center)
        {
            this.center = center;
            countPoints = 0;
        }

        public Point CalcPointSpiral()
        {
            var p = Step * countPoints / TwoPi;
            var x = (int)(p * Math.Cos(countPoints + 1)) + center.X;
            var y = (int)(p * Math.Sin(countPoints + 1)) + center.Y;

            countPoints++;

            return new Point(x, y);
        }
    }
}