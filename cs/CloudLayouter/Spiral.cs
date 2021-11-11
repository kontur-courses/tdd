using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace CloudLayouter
{
    public class Spiral : IEnumerable<Point>
    {
        private readonly Point center;
        private readonly double angleStep;
        private double angle;
        private readonly double distanceBetweenLoops;
        
        public Spiral(
            Point center, 
            double angleStep = Math.PI/180, 
            double distanceBetweenLoops = 1) 
        {
            this.center = center;
            this.angleStep = angleStep;
            this.distanceBetweenLoops = distanceBetweenLoops;
        }

        private IEnumerable<Point> PontGenerator()
        {
            while (true)
            {
                var x = Convert.ToInt32(distanceBetweenLoops * angle * Math.Cos(angle) + center.X);
                var y = Convert.ToInt32(distanceBetweenLoops * angle * Math.Sin(angle) + center.Y);
                angle += angleStep;

                yield return new Point(x, y);
            }
        }

        public IEnumerator<Point> GetEnumerator()
        {
            return PontGenerator().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}