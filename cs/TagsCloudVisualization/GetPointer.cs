using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class GetPointer
    {
        private readonly Point center;
        private double angle;
        private int radius;
        private double dangle { get => Math.PI / (2 * radius); }
        internal GetPointer(Point center) => this.center = center;
        internal Point GetNextPoint()
        {
            var x = (int)(radius * Math.Cos(angle));
            var y = (int)(radius * Math.Sin(angle));
            ChangePolarCoordinate();
            return new Point(x + center.X, y + center.Y);
        }
        private void ChangePolarCoordinate()
        {
            if (radius == 0 || angle > 2 * Math.PI)
            {
                radius++;
                angle = -dangle;
            }
            angle += dangle;
        }
    }
}
