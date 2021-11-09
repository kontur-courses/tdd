using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleFExtensions
    {
        public static PointF GetCenter(this RectangleF rectangle)
        {
            return new PointF(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        public static IEnumerable<PointF> GetPoints(this RectangleF rectangle)
        {
            yield return rectangle.Location;
            yield return new PointF(rectangle.Right, rectangle.Top);
            yield return new PointF(rectangle.Right, rectangle.Bottom);
            yield return new PointF(rectangle.Left, rectangle.Bottom);
        }

        public static bool Contacts(this RectangleF rectangle, RectangleF other)
        {
            return rectangle.Top == other.Top && (rectangle.Left == other.Right || rectangle.Right == other.Left) ||
                rectangle.Left == other.Left && (rectangle.Top == other.Bottom || rectangle.Bottom == other.Top);
        }
        
        
    }
}