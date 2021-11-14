using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualizer
{
    public static class RectangleExtensions
    {
        public static bool IntersectsWith(this Rectangle rectangle, IEnumerable<Rectangle> others)
        {
            return others.Select(x => x.IntersectsWith(rectangle)).Any(x => x);
        }
        public static string ToJson(this Rectangle rectangle)
        {
            var sb = new StringBuilder();
            sb.Append("{\"X\":" + rectangle.X + ",");
            sb.Append("\"Y\":" + rectangle.Y + ",");
            sb.Append("\"Width\":" + rectangle.Width + ",");
            sb.Append("\"Height\":" + rectangle.Height + "}");
            return sb.ToString();
        }
        public static double GetDiagonalLength(this Rectangle rect1)
        {
            var leftBottomCorner1 = new Point(rect1.Left, rect1.Bottom);
            var rightUpperCorner1 = new Point(rect1.Right, rect1.Top);
            return leftBottomCorner1.GetDistanceTo(rightUpperCorner1);
        }
    }
}
