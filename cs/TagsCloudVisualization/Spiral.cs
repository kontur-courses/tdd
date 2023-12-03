using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly Point center;
        private readonly double offsetAngle;
        private double angle;

        public Spiral(Point center, double offsetAngle) 
        {
            this.center = center;
            this.offsetAngle = offsetAngle;
        }

        public Point GetNextPointOnSpiral()
        {
            angle += offsetAngle;
            return ConvertFromPolarCoordinates();
        }

        private Point ConvertFromPolarCoordinates()
        {
            var x = (int)Math.Ceiling(center.X + Math.Cos(angle) * angle);
            var y = (int)Math.Ceiling(center.Y + Math.Sin(angle) * angle);
            return new Point(x, y);
        }
    }
}
