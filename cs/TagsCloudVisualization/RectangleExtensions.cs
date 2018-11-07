using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Rectangle Moved(this Rectangle rect, Direction moveDirection)
        {
            var (dx, dy) = moveDirection.GetOffset();
            rect.X += dx;
            rect.Y += dy;

            return rect;
        }

        public static Rectangle Moved(this Rectangle rect, Point offset)
        {
            rect.X += offset.X;
            rect.Y += offset.Y;

            return rect;
        }
    }
}
