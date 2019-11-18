using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static Point GetRectangleCenter(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2,
                     rect.Top + rect.Height / 2);
        }
    }
}
