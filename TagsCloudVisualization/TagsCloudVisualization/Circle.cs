using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Circle
    {
        public Point Center { get; }
        public List<Point> CirclePoints { get; } = new List<Point>();

        public int Radius { get; private set; }

        public Circle(Point center)
        {
            Center = center;
            Radius = 1;
            CalculateCirclePoints();          
        }

        private void CalculateCirclePoints()
        {
            CirclePoints.Clear();
            var topArc = Enumerable.Range(-Radius, Radius*2)
                .Select(i =>
                    new Point(i, (int) Math.Sqrt(Radius * Radius - (Center.X - i) * (Center.X - i)) + Center.Y))
                .ToList();
            CirclePoints.AddRange(topArc);
            CirclePoints.AddRange(VerticalFlip(topArc));
        }

        private IEnumerable<Point> VerticalFlip(IEnumerable<Point> arc)
        {
            return arc.Select(point => new Point(point.X, -point.Y)).ToList();
        }

        public void IncrementRadius(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative.");
            Radius = Radius + value;
            CalculateCirclePoints();
        }

    }
}
