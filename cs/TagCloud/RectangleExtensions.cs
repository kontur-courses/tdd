using System.Drawing;

namespace TagCloud
{
    public static class RectangleExtensions
    {
        public static Point GetCenter(this Rectangle rectangle)
        {
            return new Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        }

        public static Rectangle MoveAlongVector(this Rectangle rectangle, Vector vector)
        {
            return new Rectangle(new Point(rectangle.X + vector.X, rectangle.Y + vector.Y), rectangle.Size);
        }
    }
}