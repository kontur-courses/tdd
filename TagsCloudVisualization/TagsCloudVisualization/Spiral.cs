using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        private double radius = 0;
        private double angle = 0;
        private HashSet<Point> points = new HashSet<Point>();
        
        public Spiral(Point center)
        {
            this.center = center;
        }
        
        public IEnumerable<Point> GetPoints()
        {
            while (true)
            {
                var point = ConvertingBetweenPolarToCartesianCoordinates(radius, angle);
                if (! points.Contains(point))
                {
                    points.Add(point);
                    point.Offset(center);
                    yield return point;
                }
                radius = angle > Math.PI * 2 ? radius + 1: radius;
                angle = angle > Math.PI * 2 ? angle - Math.PI * 2 : angle + 0.1;
            }
        }

        public static Point ConvertingBetweenPolarToCartesianCoordinates(double radius, double angle)
        {
            var x = (int) Math.Round(radius * Math.Cos(angle));
            var y = (int) Math.Round(radius * Math.Sin(angle));
            return new Point(x,y);
        }
    }
}