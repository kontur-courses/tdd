using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Spiral : ISpiral
    {
        public Spiral(double radius = 1, double angleStep = 15)
        {
            Radius = radius;
            AngleStep = angleStep;
        }

        private const int maxAngle = 360;
        private const int radiusStep = 1;

        public double Radius { get; set; }
        public double AngleStep { get; set; }

        private double angle;
        public Point GetNextPosition()
        {
            var newPoint = new Point
                (
                    (int)(Radius * Math.Cos(ConvertFromDegreesToRadians(angle))),
                    (int)(Radius * Math.Sin(ConvertFromDegreesToRadians(angle)))
                );

            UnfoldSpiral();

            return newPoint;
        }

        private void UnfoldSpiral()
        {
            angle += AngleStep;
            if (angle >= maxAngle)
            {
                angle -= maxAngle;
                Radius += radiusStep;
            }
        }

        private double ConvertFromDegreesToRadians(double degrees)
        {
            return 2 * Math.PI / (maxAngle / degrees);
        }

    }
}
