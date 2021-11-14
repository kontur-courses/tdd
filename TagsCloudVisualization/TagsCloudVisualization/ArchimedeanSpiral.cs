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
        private double _radius;

        /// <summary>
        /// Текущий угол в радианах
        /// </summary>
        private double _phi;

        public ArchimedeanSpiral() : base() 
        {
            _radius = 1;
            _phi = 0;
        }

        public ArchimedeanSpiral(Point center) : base(center)
        {
            _radius = 1;
            _phi = 0;
        }

        public override IEnumerable<Point> GetDiscretePoints(double deltaAngle = 0.01)
        {
            while (true)
            {
                var rho = _phi * _radius / (2 * Math.PI);
                var cartesian = CoordinatesConverter.ToCartesian(rho, _phi);
                var point = new Point(cartesian.X + OffsetX, cartesian.Y + OffsetY);

                _phi += deltaAngle;
                yield return point;
            }
        }
    }
}
