using System.Drawing;
using TagsCloudVisualization.Interfaces;

namespace TagsCloudVisualization
{
    public class Spiral : ISpiral
    {
        private readonly Point center;
        private readonly double deltaAngle;
        private readonly double deltaRadius;

        public Spiral(Point center, double deltaAngle, double deltaRadius)
        {
            this.center = center;
            this.deltaAngle = deltaAngle;
            this.deltaRadius = deltaRadius;
        }

        public IEnumerable<Point> GetPointsOnSpiral()
        {
            var angle = 0.0;
            var radius = 0.0;
            while (true)
            {
                yield return ConvertFromPolarCoordinates(angle, radius);
                angle += deltaAngle;
                radius += deltaRadius;
            }
        }

        public Point ConvertFromPolarCoordinates(double angle, double radius)
        {
            var x = (int)Math.Ceiling(center.X + Math.Cos(angle) * radius);
            var y = (int)Math.Ceiling(center.Y + Math.Sin(angle) * radius);
            return new Point(x, y);
        }
    }
}
