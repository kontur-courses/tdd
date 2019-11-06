using System.Drawing;

namespace TagsCloudVisualization
{
    public class GeometryHelper
    {
        public static Rectangle GetRectangleFromCenterPoint(Point center, Size size)
        {
            return new Rectangle(center.X - size.Width / 2, center.Y + size.Height / 2, size.Width, size.Height);
        }

        public static int GetSquare(Size size)
        {
            return size.Width * size.Height;
        }
    }
}