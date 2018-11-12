using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtensions
    {
        public static Rectangle Moved(this Rectangle rectangle, Point offset)
        {
            rectangle.X += offset.X;
            rectangle.Y += offset.Y;

            return rectangle;
        }
    }
}
