using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class RecatngleExtensions
    {
        public static bool IntersectWith(this IEnumerable<Rectangle> rects, Rectangle rect) => rects.Any(r => r.IntersectsWith(rect));


    }
}
