using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Density
{
    internal interface IRectangleFCompactor
    {
        RectangleF Compact(RectangleF candidate, List<RectangleF> placed, PointF center);
    }
}
