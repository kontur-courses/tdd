using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Point Center(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,rect.Top + rect.Height / 2);
        }

        public static bool CanBeShiftedToPointX(this Rectangle rect, Point point)
        {
            return (point.X - 1 > rect.Center().X || point.X + 1 < rect.Center().X);
        }
        public static bool CanBeShiftedToPointY(this Rectangle rect, Point point)
        {
            return (point.Y - 1 > rect.Center().Y || point.Y + 1 < rect.Center().Y);
        }

        public static bool AreIntersected(this Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Left || r1.Right >= r2.Left)
                   && (r1.Top >= r2.Top || r1.Top + r1.Height >= r2.Top)
                   && (r2.Left >= r1.Left || r2.Right >= r1.Left)
                   && (r2.Top >= r1.Top || r2.Top + r2.Height >= r1.Top);
        }

        public static bool AreIntersected(this Rectangle rectangle, List<Rectangle> rectangles)
        {
            if (rectangles is null) return false;
            return rectangles.Any(rect => AreIntersected(rectangle, rect));
        }

        public static bool AreIntersected(this List<Rectangle> rectangles)
        {
            for (var i = 0; i < rectangles.Count - 1; i++)
            {
                for (var j = i + 1; j < rectangles.Count; j++)
                {
                    if (AreIntersected(rectangles[i], rectangles[j]))
                        return true;
                }
            }
            return false;
        }
    }
}
