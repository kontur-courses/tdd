using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        public static bool IsIntersectsOthersRectangles(this Rectangle rectangle, IEnumerable<Rectangle> rectangles)
        {
            if (rectangles == null)
                throw new ArgumentNullException();
            foreach (var rect in rectangles)
            {
                if (rect.IntersectsWith(rectangle))
                    return true;
            }
            return false;
        }
    }
}
