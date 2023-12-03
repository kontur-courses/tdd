using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        private readonly double deltaAngle;
        private readonly double deltaRadius;
        private double radius;
        private double angle;

        public Spiral(Point center, double deltaAngle, double deltaRadius)
        {
            this.center = center;
            this.deltaAngle = deltaAngle;
            this.deltaRadius = deltaRadius;
        }

        public Point GetNextPointOnSpiral()
        {
            angle += deltaAngle;
            radius += deltaRadius;
            return ConvertFromPolarCoordinates();
        }

        private Point ConvertFromPolarCoordinates()
        {
            var x = (int)Math.Ceiling(center.X + Math.Cos(angle) * radius);
            var y = (int)Math.Ceiling(center.Y + Math.Sin(angle) * radius);
            return new Point(x, y);
        }
    }
}
