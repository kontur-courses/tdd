using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Circle
    {
        public Point Center { get; }

        public int Radius { get; private set; }

        public Circle(Point center)
        {
            Center = center;
            Radius = 1;
        }

        public IEnumerable<Point> GetCirclePoints()
        {
            foreach (var i in Enumerable.Range(-Radius, Radius * 2))
            {
                    yield return new Point(i,
                        (int) Math.Sqrt(Radius * Radius - (Center.X - i) * (Center.X - i)) + Center.Y);
            }           
        }


        public void IncrementRadius(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative.");
            Radius = Radius + value;
        }

    }
}
