using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiralGenerator
    { 
        public static IEnumerable<PointF> GetArchimedeanSpiralGenerator(PointF center, float step, float angleDeltaInRadians)
        {
            var currentAngle = 0f;
            while (true)
            {
                yield return GetPointFromAngle(center, step, currentAngle);
                currentAngle += angleDeltaInRadians;
            }
        }

        protected static PointF GetPointFromAngle(PointF center, float step, float angleInRadians)
        {
            var distance = angleInRadians * step / (2 * Math.PI);
            var xCoordinate = distance * Math.Cos(angleInRadians);
            var yCoordinate = distance * Math.Sin(angleInRadians);
            return new PointF((float)xCoordinate, (float)yCoordinate);
        }
    }
}
