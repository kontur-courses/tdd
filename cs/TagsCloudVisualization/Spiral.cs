using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly int parameter;
        private readonly double stepInRadians;

        public Spiral(int parameter, int stepInDegrees)
        {
            this.parameter = parameter;
            stepInRadians = stepInDegrees * Math.PI / 180;
        }

        public IEnumerable<Point> GetNextPoint()
        {
            for (double phi = 0; ; phi += stepInRadians)
            {
                var r = parameter * phi;
                yield return GeometryUtils.ConvertPolarToIntegerCartesian(r, phi);
            }
        }
    }
}