using System.Drawing;

namespace TagsCloudVisualization
{
    public static class SquareRectangle
    {
        public static double Square(this Rectangle rectangle)
        {
            return rectangle.Height * rectangle.Width;
        }
    }
}