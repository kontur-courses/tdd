using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleFExtensions
    {
        public static bool IntersectsWithAny(this RectangleF rect, IEnumerable<RectangleF> rectangles)
            => rectangles.Any(r => r.IntersectsWith(rect));

    }
}