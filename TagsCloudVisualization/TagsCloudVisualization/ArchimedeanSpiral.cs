using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Спираль Архимеда
    /// </summary>
    public class ArchimedeanSpiral : Spiral
    {
        /// <summary>
        /// Радиус витков
        /// </summary>
        private double Radius;

        /// <summary>
        /// Текущий угол в радианах
        /// </summary>
        private double Phi;

        public ArchimedeanSpiral() : base() 
        {
            Radius = 1;
            Phi = 0;
        }

        public ArchimedeanSpiral(Point center) : base(center)
        {
            Radius = 1;
            Phi = 0;
        }

        public override IEnumerable<Point> GetDiscretePoints(double deltaAngle = 0.01)
        {
            while (true)
            {
                var rho = Phi * Radius / (2 * Math.PI);
                var cartesian = CoordinatesConverter.ToCartesian(rho, Phi);
                var point = new Point(cartesian.X + OffsetX, cartesian.Y + OffsetY);

                Phi += deltaAngle;
                yield return point;
            }
        }
    }
}
