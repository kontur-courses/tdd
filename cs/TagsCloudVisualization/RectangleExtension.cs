using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static bool IntersectsWithAny(this Rectangle rect, List<Rectangle> rectangles)
        {
            foreach (var shape in rectangles)
                if (rect.IntersectsWith(shape))
                    return true;
            return false;
        }
    }
}
