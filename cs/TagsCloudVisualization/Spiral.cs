using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Spiral
    {
        private readonly int parameter;
        private readonly double stepInRadians;
        private double phi;

        public Spiral(int parameter, int stepInDegrees)
        {
            this.parameter = parameter;
            stepInRadians = stepInDegrees * Math.PI / 180;
        }

        public Point GetNextPoint()
        {
            var r = parameter * phi;
            var point =  GeometryUtils.ConvertPolarToIntegerCartesian(r, phi);
            phi += stepInRadians;
            return point;
        }
    }
}