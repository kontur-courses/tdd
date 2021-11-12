using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.PointGenerator
{
    public class Spiral
    {
        private float spiralPitch;
        private readonly float anglePitch;
        private Cache cache = new Cache();

        public Spiral(float spiralPitch, float anglePitch)
        {
            this.spiralPitch = spiralPitch;
            this.anglePitch = anglePitch;
        }

        public IEnumerable<PointF> GetPoints(PointF center, Size size)
        {
            spiralPitch = Math.Min(size.Height, size.Width);
            foreach (var polarCoordinate in GetArchimedeanSpiral(cache.GetParameter(size)))
            {
                cache.UpdateParameter(size, polarCoordinate.angle);
                var cartesianPoint = PolarToCartesian(polarCoordinate.radius, polarCoordinate.angle);
                yield return new PointF(cartesianPoint.x + center.X, cartesianPoint.y + center.Y);
            }
        }

        private IEnumerable<(float radius, float angle)> GetArchimedeanSpiral(float currentAngle)
        {
            while (true)
            {
                var radius = (float)(spiralPitch * currentAngle / (2 * Math.PI));
                yield return (radius, currentAngle);
                currentAngle += anglePitch;
            }
        }

        private static (float x, float y) PolarToCartesian(float r, float angle)
        {
            var x = (float)(r * Math.Cos(angle));
            var y = (float)(r * Math.Sin(angle));
            return (x, y);
        }
    }
}