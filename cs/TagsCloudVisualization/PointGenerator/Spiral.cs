using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.PointGenerator
{
    public class Spiral : IPointGenerator
    {
        private double spiralPitch;
        private readonly float anglePitch;
        private readonly double pitchCoefficient;
        private readonly PointF center;
        private readonly Cache cache = new Cache();

        public Spiral(float anglePitch, double densityCoefficient, PointF center)
        {
            this.anglePitch = anglePitch;
            this.center = center;
            pitchCoefficient = 20 * densityCoefficient * densityCoefficient;
        }

        public IEnumerable<PointF> GetPoints(Size size)
        {
            spiralPitch = Math.Min(size.Height, size.Width)/pitchCoefficient;
            foreach (var (radius, angle) in GetArchimedeanSpiral(cache.GetParameter(size)))
            {
                cache.UpdateParameter(size, angle);
                var (x, y) = PolarToCartesian(radius, angle);
                yield return new PointF(x + center.X, y + center.Y);
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
            // ReSharper disable once IteratorNeverReturns
        }

        private static (float x, float y) PolarToCartesian(float r, float angle)
        {
            var x = (float)(r * Math.Cos(angle));
            var y = (float)(r * Math.Sin(angle));
            return (x, y);
        }
    }
}