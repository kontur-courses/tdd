using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private Point Center { get; }
        private IEnumerator<Point> PointsGenerator { get; }
        private double DistanceBetweenLoops { get; }

        public ArchimedeanSpiral(Point center, double distanceBetweenLoops)
        {
            Center = center;
            PointsGenerator = GetPoints().GetEnumerator();

            if (distanceBetweenLoops <= 0)
            {
                throw new ArgumentException("distanceBetweenLoops should not be negative or zero");
            }

            DistanceBetweenLoops = distanceBetweenLoops;
        }

        public Point GetNextPoint()
        {
            PointsGenerator.MoveNext();
            return PointsGenerator.Current;
        }

        private IEnumerable<Point> GetPoints()
        {
            var x = Center.X;
            var y = Center.Y;
            var i = 0;

            while (true)
            {
                x += (int)(DistanceBetweenLoops * i * Math.Cos(i));
                y += (int)(DistanceBetweenLoops * i * Math.Sin(i));
                i++;

                yield return new Point(x, y);
            }
        }
    }
}