using System.Drawing;

namespace TagsCloudVisualization
{
    public static partial class RectangleExtension
    {
        public static Rectangle ShiftRectangleToTopLeftCorner(this Rectangle rectangle)
        {
            var center = new Point(rectangle.Location.X - rectangle.Width / 2, rectangle.Location.Y - rectangle.Height / 2);
            return new Rectangle(center, rectangle.Size);
        }

        public static Rectangle ShiftRectangleToBitMapCenter(this Rectangle rectangle, Bitmap bitmap)
        {
            var center = new Point(rectangle.X + bitmap.Width/2, rectangle.Y + bitmap.Height/2);
            return new Rectangle(center, rectangle.Size);
        }
    }
}
