using System.Drawing;

namespace TagCloud
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            var centerPoint = rectangle.Location.MoveOn(rectangle.Width / 2, rectangle.Height / 2);

            return centerPoint;
        }
    }
}