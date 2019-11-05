using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class PointExtensions
    {
        private const double RadiansInOneDegree = Math.PI / 180;

        public static PointF FromPolar(float rho, float phi)
        {
            var phiInRadian = phi * RadiansInOneDegree;
            return new PointF(
                (float) (rho * Math.Cos(phiInRadian)),
                (float) (rho * Math.Sin(phiInRadian)));
        }
    }
}