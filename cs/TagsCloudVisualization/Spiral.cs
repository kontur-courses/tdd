using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        public Point Center { get; }
        private readonly double step;
        private readonly double parameter;

        public Spiral(Point center, double step = 0.3, double parameter = 2*Math.PI)
        {
            if (step == 0 || parameter == 0)
                throw new ArgumentException("step and parameter must not be zero");

            Center = center;
            this.step = step;
            this.parameter = parameter / (2 * Math.PI);
        }

        public List<Point> GetPoints(int count)
        {
            var points = new List<Point>();
            int x;
            int y;
            double angle = 0.0;

            var currentPoint = Center;

            for (int i = 0; i < count;)
            {
                x = Center.X + (int)Math.Round(parameter * angle * Math.Cos(angle));
                y = Center.Y + (int)Math.Round(parameter * angle * Math.Sin(angle));

                var nextPoint = new Point(x, y);
                if (!nextPoint.Equals(currentPoint))
                {
                    points.Add(new Point(x, y));
                    i++;
                }

                angle += step;
                currentPoint = nextPoint;
            }

            return points;
        }
    }
}