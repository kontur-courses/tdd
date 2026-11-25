using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Density
{
    internal class CenterwardCompactor : IRectangleFCompactor
    {
        private readonly float step;

        public CenterwardCompactor(float step = 1)
        {
            if (step <= 0) throw new ArgumentException(
                    $"CenterwardCompactor step must be positive, got {step}.");
            this.step = step;
        }

        public RectangleF Compact(RectangleF candidate, List<RectangleF> placed, PointF center)
        {
            //скорре всего через бин поиск буду искать свободную позицию
            return candidate;
        }
    }
}
