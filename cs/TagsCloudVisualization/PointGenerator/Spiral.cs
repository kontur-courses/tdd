using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.PointGenerator
{
    public class Spiral : IPointGenerator
    {
        private readonly float spiralPitch;
        private readonly float anglePitch;
        private static Random random;
        public Spiral(float spiralPitch, float anglePitch)
        {
            this.spiralPitch = spiralPitch;
            this.anglePitch = anglePitch;
            random = new Random();
        }
        public IEnumerable<PointF> GetPoints(PointF center)
        {
            var startAngle = (float)(random.NextDouble()*Math.PI/2);
            return GetArchimedeanSpiral(startAngle).Select(point => new PointF(point.X + center.X, point.Y + center.Y));
        }

        private IEnumerable<PointF> GetArchimedeanSpiral(float startAngle)
        {
            var currentAngle = startAngle;
            while (true)
            {
                var r = (float)(spiralPitch * currentAngle / (2 * Math.PI));
                var (x,y) = PolarToCartesian(r, currentAngle);
                yield return new PointF(x, y);
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