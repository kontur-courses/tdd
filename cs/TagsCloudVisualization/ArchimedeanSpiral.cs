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
        private double CenterOffset { get; }

        public ArchimedeanSpiral(Point center, double distanceBetweenLoops, double centerOffset)
        {
            Center = center;
            PointsGenerator = GetPoints().GetEnumerator();
            DistanceBetweenLoops = distanceBetweenLoops;
            CenterOffset = centerOffset;
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
                var angle = 0.1 * i;
                i++;

                x += (int)((CenterOffset + DistanceBetweenLoops * angle) * Math.Cos(angle));
                y += (int)((CenterOffset + DistanceBetweenLoops * angle) * Math.Sin(angle));

                yield return new Point(x, y);
            }
        }
    }
}