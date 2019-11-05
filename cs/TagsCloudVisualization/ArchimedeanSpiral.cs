using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        private readonly Point center;

        private readonly double stepLength;
        private readonly int amountOfSubsteps;
        private readonly double spiralAngleDelta;
        private double currentSpiralAngle;

        public ArchimedeanSpiral(Point center, double stepLength, int amountOfSubsteps)
        {
            if (stepLength <= 0)
                throw new ArgumentException("Spiral step length must be a positive number!");
            if (amountOfSubsteps <= 0)
                throw new ArgumentException("Amount of steps should be a positive number!");
            this.center = center;
            this.stepLength = stepLength;
            this.amountOfSubsteps = amountOfSubsteps;
            spiralAngleDelta = 2 * Math.PI / amountOfSubsteps;
        }

        public Point CalculateNextPoint()
        {
            var angle = currentSpiralAngle;
            var radiusLength = stepLength * angle / (2 * Math.PI);
            currentSpiralAngle += spiralAngleDelta;

            var x = (int)(radiusLength * Math.Cos(angle) + center.X);
            var y = (int)(radiusLength * Math.Sin(angle) + center.Y);
            return new Point(x, y);
        }
    }
}
