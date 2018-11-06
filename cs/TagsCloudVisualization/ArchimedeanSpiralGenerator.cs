using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiralGenerator
    {
        private float currentAngle;
        private PointF center;
        private float step;
        private float angleDeltaInRadians;

        public ArchimedeanSpiralGenerator(PointF center, float step, float angleDeltaInRadians)
        {
            this.center = center;
            this.step = step;
            this.angleDeltaInRadians = angleDeltaInRadians;
            currentAngle = 0f;
        }

        public PointF GetNextPoint()
        { 
            var distance = currentAngle * step / (2 * Math.PI);
            var xCoordinate = distance * Math.Cos(currentAngle);
            var yCoordinate = distance * Math.Sin(currentAngle);

            currentAngle += angleDeltaInRadians;
            return new PointF((float)xCoordinate, (float)yCoordinate);
        }
    }
}
