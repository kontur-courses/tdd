using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral : IPointDistributor
    {
        private readonly int step = 1;
        private double angle;

        public Point GetPosition(Cloud cloud, Size rectangleSize, double deltaAngle = 0.1)
        {
            angle += deltaAngle;

            var k = step / (2 * Math.PI);
            var radius = k * angle;

            var position = new Point(
                cloud.Center.X + (int)(Math.Cos(angle) * radius),
                cloud.Center.Y + (int)(Math.Sin(angle) * radius));

            return position;
        }
    }
}
