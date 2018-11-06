using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Provides Archimedean spiral
    /// </summary>
    public class Spiral
    {
        private readonly double spiralStep;
        private readonly Point spiralCenter;

        public Spiral(Point spiralCenter, double spiralStep)
        {
            this.spiralStep = spiralStep;
            this.spiralCenter = spiralCenter;
        }

        public IEnumerable<Point> GetSpiralPoints()
        {
            double alphaAngle = 1;

            while (true)
            {
                var p = spiralStep / (2 * Math.PI) * alphaAngle;
                var dx = Math.Cos(alphaAngle) * p;
                var dy = Math.Sin(alphaAngle) * p;

                yield return new Point(spiralCenter.X + (int)dx, spiralCenter.Y + (int)dy);

                alphaAngle += 0.1;
            }
        }
    }
}