using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Spiral
{
    internal class ArchimedeanSpiral : ISpiral
    {
        private readonly PointF center;
        private readonly float step;
        private readonly float angleStep;

        private float currentAngle;
        private float CurrentRadius => (float)(step * currentAngle / (2 * Math.PI));

        private PointF CartesianPoint => new(
            center.X + CurrentRadius * (float)Math.Cos(currentAngle),
            center.Y + CurrentRadius * (float)Math.Sin(currentAngle));

        public ArchimedeanSpiral(PointF center, float step = 5, float angleStep = 0.01f)
        {
            if (step <= 0) throw new ArgumentException(
                    $"Spiral step must be positive, got {step}.");
            if (angleStep <= 0) throw new ArgumentException(
                    $"Spiral angleStep must be positive, got {angleStep}.");

            this.center = center;
            this.step = step;
            this.angleStep = angleStep;
            currentAngle = 0;
        }

        public PointF GetNextPoint()
        {
            currentAngle += angleStep;

            return CartesianPoint;
        }
    }
}
