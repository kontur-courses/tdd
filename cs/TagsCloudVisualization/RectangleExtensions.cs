using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this IEnumerable<Rectangle> source, Rectangle other)
            => source.Any(x => x.IntersectsWith(other));
    }
}