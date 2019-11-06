using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class Spiral
    {
        private readonly double turnsDistance;
        private readonly double deltaAngle;

        public Spiral() : this(1, 10)
        {
        }

        public Spiral(float turnsDistance) : this(1, 10)
        {
        }
        /// <summary>
        /// Create new Spiral
        /// </summary>
        /// <param name="turnsDistance">distance between different turns</param>
        /// <param name="deltaAngle">Angle between next and previous point in degrees</param>
        public Spiral(double turnsDistance, double deltaAngle)
        {
            this.turnsDistance = turnsDistance / 2 / Math.PI;
            this.deltaAngle = deltaAngle * Math.PI / 180;
        }

        public IEnumerator<Point> GetNextPointOnSpiral()
        {
            var angle = 0d;
            while (true)
            {
                var distance = turnsDistance * angle;
                yield return getPointFromPolarCoordinates(distance, angle);
                angle += deltaAngle;
            }
        }

        private Point getPointFromPolarCoordinates(double r, double angle)
        {
            return new Point((int)(r * Math.Cos(angle)), (int)(r * Math.Sin(angle)));
        }
    }
}
