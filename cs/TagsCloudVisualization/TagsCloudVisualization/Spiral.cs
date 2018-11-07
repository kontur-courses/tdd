using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private double currentAngle = 0;
        private double angleRotate = 0.05;
        private readonly Point center;

        public Spiral(Point center)
        {
            this.center = center;
        }
        
        public IEnumerable<Point> GetNextPoint()
        {
            while (true)
            {
                var x = center.X + (int) (currentAngle * Math.Cos(currentAngle));
                var y = center.Y + (int) (currentAngle * Math.Sin(currentAngle));
                yield return new Point(x, y);
                UpdateCurrentAngle();
            }
        }

        private void UpdateCurrentAngle()
        {
            currentAngle += angleRotate;
        }
    }
}