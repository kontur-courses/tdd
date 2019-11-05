using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral : ISpiral
    {
        private const float StepPhi = 0.05f;
        private float phi;
        private float rho;
        private float k;
        
        public ArchimedeanSpiral(float alpha)
        {
            phi = 0;
            rho = 0;
            k = (float) (alpha / (2 * Math.PI));
        }

        public PointF GetNextPoint()
        {
            var nextPoint = new PointF(rho, phi);
            phi += StepPhi;
            rho = k * phi;
            return PointExtensions.FromPolar(rho, phi);
        }
    }
}