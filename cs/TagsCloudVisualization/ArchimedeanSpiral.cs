using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class ArchimedeanSpiral : ISpiral
    {
        public IEnumerable<Point> GetPoints()
        {
            double defaultStep = 0.05;
            double t = 0;
            double r = 1;
            while (true)
            {
                t += defaultStep * 2 * r;
                yield return new Point((int)(1.5 * t * Math.Cos(t)), (int)(t * Math.Sin(t)));
            }
        }
    }
}
