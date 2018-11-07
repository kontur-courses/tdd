using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Circle
    {
        public Point Center { get; }
        public List<Point> RightHalfPoints { get; private set; }
        public List<Point> LeftHalfPoints { get; private set; }

        public int Radius { get; private set; } = 1;

        public Circle(Point center)
        {
            Center = center;
            Calculate();
        }

        private void Calculate()
        {
            var quater = new List<Point>();
            for (var i = 0; i <= Radius; i++)
            {
                var y = Math.Sqrt(Radius * Radius - (Center.X - i) * (Center.X - i)) + Center.Y;
                quater.Add(new Point(i, (int)y));
            }
            RightHalfPoints = FlipDown(quater);
            LeftHalfPoints = HorizontalReflect(RightHalfPoints);
        }

        private List<Point> FlipDown(IEnumerable<Point> arc)
        {
            var result = new List<Point>();
            result.AddRange(arc);
            result.AddRange(arc.Select(point => new Point(point.X, -point.Y)).ToList());
            return result;
        }

        private List<Point> HorizontalReflect(IEnumerable<Point> arc)
        {
            return arc.Select(point => new Point(-point.X, point.Y)).ToList();
        }

        public void IncrementRadius(int value)
        {
            if (value < 0)
                throw new ArgumentException("Value cannot be negative.");
            Radius = Radius + value;
            Calculate();
        }

    }
}
