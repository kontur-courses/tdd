using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

// ReSharper disable once CheckNamespace
namespace TagCloudVisualization
{
    public class Spiral : ICurve
    {
        private readonly float distanceBetweenLoops;
        private readonly float angleIncrement;

        public Point Center { get; private set; }

        public Spiral(float distanceBetweenLoops, Point center, float angleIncrement = 0.02f)
        {
            if (distanceBetweenLoops == 0)
            {
                throw new ArgumentException(
                    $"distanceBetweenLoops cannot be zero");
            }

            if (angleIncrement == 0)
            {
                throw new ArgumentException(
                    $"angleIncrement cannot be zero");
            }

            this.angleIncrement = angleIncrement;
            this.distanceBetweenLoops = distanceBetweenLoops / (float)(2 * Math.PI);
            this.Center = center;
        }

        private Point GetArchimedeanSpiralPoint(float angle)
        {
            return new Point((int)(Center.X + distanceBetweenLoops * angle * Math.Cos(angle)),
                (int)(Center.Y - distanceBetweenLoops * angle * Math.Sin(angle)));
        }

        public IEnumerator<Point> GetEnumerator()
        {
            for (var angle = 0f;; angle += angleIncrement)
            {
                yield return GetArchimedeanSpiralPoint(angle);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ChangeCenterPoint(Point newCenter)
        {
            Center = newCenter;
        }
    }
}