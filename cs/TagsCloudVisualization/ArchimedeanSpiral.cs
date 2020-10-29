using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class ArchimedeanSpiral
    {
        public Point Center { get; }
        public double Density { get; }

        public ArchimedeanSpiral(Point center, double density)
        {
            Center = center;

            if (density <= 0)
            {
                throw new ArgumentException("density cant be negative or zero");
            }

            Density = density;
        }

        public IEnumerator<PointF> GetNextPoint()
        {
            yield return new PointF();
        }
    }
}