using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace CircularCloudLayouter
{
    public class Spiral : IEnumerable<Point>
    {
        private float k;
        private Point center;
        private float stepAngle;
        private float angle;

        public Spiral(float k, Point center, float stepAngle = 0.02f)
        {
            this.stepAngle = stepAngle;
            this.k = k;
            this.center = center;
            angle = 0f;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            while (true)
            {
                angle += stepAngle;
                var countCircle = (int)(stepAngle / (Math.PI));
                stepAngle = countCircle != 0 ? stepAngle / countCircle : stepAngle;
                yield return GetPointArchimedeanSpiral(angle);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private Point GetPointArchimedeanSpiral(float angle)
        {
            return new Point((int)(center.X + k * angle * Math.Cos(angle)), (int)(center.Y - k * angle * Math.Sin(angle)));
        }
    }
}