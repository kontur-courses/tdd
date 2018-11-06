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
    }
}
