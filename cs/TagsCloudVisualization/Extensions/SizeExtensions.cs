using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class SizeExtensions
    {
        public static Size Multiply(this Size size, double number)
        {
            return new Size((int)(size.Width * number), (int)(size.Height * number));
        }
    }
}