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
        public Spiral(float spiralPitch, float anglePitch)
        {
            this.spiralPitch = spiralPitch;
            this.anglePitch = anglePitch;
        }
        public IEnumerable<PointF> GetPoints(PointF center)
        {
            return GetArchimedeanSpiral(0).Select(point => new PointF(point.X + center.X, point.Y + center.Y));
        }

        private IEnumerable<PointF> GetArchimedeanSpiral(float startAngle)
        {
            while (true)
            {
                var r = (float)(spiralPitch * startAngle / (2 * Math.PI));
                var (x,y) = PolarToCartesian(r, startAngle);
                yield return new PointF(x, y);
                startAngle += anglePitch;
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