using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public List<Point> Points { get; set; }
        public List<Point> FreePoints { get; set; }
        public readonly Point center;
        private readonly double segmentLength;
        private readonly double helixPitch;
        private double lastX, lastY;

        public Spiral(Point center, double segmentLength = 50, double helixPitch = 20)
        {
            Points = new List<Point>();
            FreePoints = new List<Point>();
            this.center = center;
            this.segmentLength = segmentLength;
            this.helixPitch = helixPitch;

            double x = 0, y = 0;
            AddPoint(this.center.X, this.center.Y);
            y += segmentLength;
            AddPoint(this.center.X + x, this.center.Y + y);
            lastX = x;
            lastY = y;
        }

        public void AddPoint(double x, double y)
        {
            var addedPoint = new Point((int)x, (int)y);
            Points.Add(addedPoint);
            FreePoints.Add(addedPoint);
        }

        public void AddOneMorePointInSpiral()
        {
            double r = Math.Sqrt(lastX * lastX + lastY * lastY);
            double tx = helixPitch * lastX + r * lastY;
            double ty = helixPitch * lastY - r * lastX;
            double tLen = Math.Sqrt(tx * tx + ty * ty);
            double k = segmentLength / tLen;
            lastX -= tx * k;
            lastY -= ty * k;
            AddPoint(center.X + lastX, center.Y + lastY);
        }
    }
}