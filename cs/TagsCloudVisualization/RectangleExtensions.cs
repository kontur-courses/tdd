using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static PointF Center(this RectangleF rect)
        {
            return new PointF(rect.Left + rect.Width / 2,rect.Top + rect.Height / 2);
        }

        public static bool CanBeShiftedToPointX(this RectangleF rect, PointF point)
        {
            return (point.X - 1 > rect.Center().X || point.X + 1 < rect.Center().X);
        }
        public static bool CanBeShiftedToPointY(this RectangleF rect, PointF point)
        {
            return (point.Y - 1 > rect.Center().Y || point.Y + 1 < rect.Center().Y);
        }

        public static bool AreIntersected(this RectangleF rectangle, List<RectangleF> rectangles)
        {
            if (rectangles is null) return false;
            return rectangles.Any(rectangle.IntersectsWith);
        }

        public static bool AreIntersected(this List<RectangleF> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (rectangles[i].IntersectsWith(rectangles[j]))
                        return true;
                }
            }
            return false;
        }
    }
}
