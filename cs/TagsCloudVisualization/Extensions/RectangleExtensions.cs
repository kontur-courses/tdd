using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class PointExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            var topLeftPoint = rectangle.Location;
            return new Point(topLeftPoint.X + rectangle.Width / 2, topLeftPoint.Y + rectangle.Height / 2);
        }
        
        public static bool IntersectsWith(this Rectangle rectangle, IReadOnlyList<Rectangle> otherRectangles)
        {
            return otherRectangles.Any(rect => rect.IntersectsWith(rectangle));
        }
    }
}