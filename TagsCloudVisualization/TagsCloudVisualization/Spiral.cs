using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        //знаяю что для многопоточного кода это уязвимое место, но иначе подает сильно производительность
        private HashSet<Point> points = new HashSet<Point>();
        private  double radius = 0.0; 
        private  double angle = 0.0;
        
        public Spiral(Point center)
        {
            this.center = center;
        }
        
//        public IEnumerable<Point> GetPoints()
//        { 
//            while (true)
//            {
//                var point = ConvertingBetweenPolarToCartesianCoordinates(radius, angle);
//                point.Offset(center);
//                yield return point;
//                radius = 2 * Math.PI / 2 * angle;
//                angle += 0.1;
//            }
//        }
        
        public IEnumerable<Point> GetPoints()
        {
            while (true)
            {
                var point = ConvertingBetweenPolarToCartesianCoordinates(radius, angle);
                point.Offset(center);
                if (! points.Contains(point))
                {
                    points.Add(point);
                    yield return point;
                }

                if (angle > Math.PI * 2)
                {
                    radius++;
                    angle -= Math.PI * 2;
                    points.Clear();
                }
                else
                    angle += 0.1;
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