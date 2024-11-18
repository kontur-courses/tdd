using System.Drawing;

namespace TagsCloudVisualization
{
    internal class Circle(Point center, float startRadius = 2.0f)
    {
        private Point _center = center;
        public float Radius { get; set; } = startRadius;

        public IEnumerable<Point> GetCoordinatesOnCircle(
            int startAngle,
            int step = 1)
        {
            for (var dAngle = 0; dAngle < 360; dAngle += step)
            {
                var angle = (startAngle + dAngle) % 360;

                double angleInRadians = angle * Math.PI / 180;
                var x = (int)(_center.X + Radius * Math.Cos(angleInRadians));
                var y = (int)(_center.Y + Radius * Math.Sin(angleInRadians));
                yield return new Point(x, y);
            }
        }
    }
}