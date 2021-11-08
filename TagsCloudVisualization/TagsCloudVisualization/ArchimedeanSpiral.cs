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
        public ArchimedeanSpiral() : base() { }

        public ArchimedeanSpiral(Point center) : base(center) { }

        public override IEnumerable<Point> GetDiscretePoints()
        {
            while (true)
            {
                var rho = Phi * Radius / (2 * Math.PI);
                var cartesian = CoordinatesConverter.ToCartesian(rho, Phi);
                var point = new Point(cartesian.X + OffsetX, cartesian.Y + OffsetY);

                Phi += 0.01;
                yield return point;
            }
        }
    }
}
