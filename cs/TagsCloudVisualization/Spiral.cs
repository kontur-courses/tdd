using System.Drawing;
using System.Runtime.InteropServices.ComTypes;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        private readonly double angleStep;
        private readonly List<Point> points;

        public Spiral(Point center, double angleStep = 0.3)
        {
            if (angleStep == 0)
                throw new ArgumentException("Angle step should not be zero");

            this.center = center;
            this.angleStep = angleStep;
            points = new List<Point>();
        }

        public List<Point> GetPoints(int count)
        {
            if (points.Count != 0)
                return points;

            double angle = 0.0;
            var currentPoint = center;

            for (int i = 0; i < count;)
            {
                var x = center.X + (int)Math.Round(angle * Math.Cos(angle));
                var y = center.Y + (int)Math.Round(angle * Math.Sin(angle));
                var nextPoint = new Point(x, y);

                if (!nextPoint.Equals(currentPoint))
                {
                    points.Add(new Point(x, y));
                    i++;
                }

                angle += angleStep;
                currentPoint = nextPoint;
            }

            return points;
        }
    }
}