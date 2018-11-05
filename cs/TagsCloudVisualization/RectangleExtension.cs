using System.Drawing;

namespace TagsCloudVisualization
{
    public static partial class RectangleExtension
    {
        public static Rectangle ShiftRectangleToBottomLeftCorner(this Rectangle rectangle)
        {
            var center = new Point(rectangle.Location.X + rectangle.Width / 2, rectangle.Location.Y + rectangle.Height / 2);
            return new Rectangle(center, rectangle.Size);
        }
    }
}
