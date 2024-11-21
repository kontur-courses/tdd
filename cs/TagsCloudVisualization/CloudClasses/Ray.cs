using System.Drawing;

namespace TagsCloudVisualization.CloudClasses
{
    public class Ray
    {
        public double Radius { get; private set; }

        //В радианах
        public double Angle { get; private set; }

        public readonly Point StartPoint;

        public Ray(Point startPoint, int radius, int angle)
        {
            StartPoint = startPoint;

            Radius = radius;

            Angle = angle;
        }

        public void Update(double deltaRadius, double deltaAngle)
        {
            Radius += deltaRadius;
            Angle += deltaAngle;
        }

        public Point EndPoint => new Point
        {
            X = StartPoint.X + (int)(Radius * Math.Cos(Angle)),
            Y = StartPoint.Y + (int)(Radius * Math.Sin(Angle))
        };
    }
}
