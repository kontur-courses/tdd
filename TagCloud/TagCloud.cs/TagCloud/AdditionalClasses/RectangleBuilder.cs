using System.Drawing;

namespace TagCloud
{
    public class RectangleBuilder
    {
        public static Rectangle CreateRectangle(Size size,Point center)
        {
            return new Rectangle(center.X - size.Width / 2, center.Y - size.Height / 2, size.Width, size.Height);
        }
    }
}