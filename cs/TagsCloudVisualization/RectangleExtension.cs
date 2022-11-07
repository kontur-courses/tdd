using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectangleExtension
    {
        public static Point GetCenter(this Rectangle rectangle) => new((rectangle.X + rectangle.Right) / 2,
            (rectangle.Y + rectangle.Bottom) / 2);

        public static int GetArea(this Rectangle rectangle) => rectangle.Height * rectangle.Width;
    }
}