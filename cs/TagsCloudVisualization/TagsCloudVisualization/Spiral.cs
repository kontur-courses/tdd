using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private double currentAngle = 0;
        private double angleRotate = 0.05;
        private double shift = 1;
        private readonly Point center;

        public Spiral(Point center)
        {
            this.center = center;
        }
        
        public IEnumerable<Point> GetNextPoint()
        {
            while (true)
            {
                var x = GetXCoordinate();
                var y = GetYCoordinate();
                var nextPointOnSpiral = new Point(x, y);
                yield return nextPointOnSpiral;
                UpdateCurrentAngle();
            }
        }

        private void UpdateCurrentAngle()
        {
            currentAngle += angleRotate;
        }

        private int GetXCoordinate() => center.X + (int) (shift * currentAngle * Math.Cos(currentAngle));
        private int GetYCoordinate() => center.Y + (int) (shift * currentAngle * Math.Sin(currentAngle));
    }
}