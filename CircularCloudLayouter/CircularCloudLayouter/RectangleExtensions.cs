using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizer
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> others)
        {
            return others.Select(x => x.IntersectsWith(rectangle)).Any(x => x);
        }
    }
}
