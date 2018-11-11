using System.Drawing;

namespace TagsCloudVisualization.Extensions
{
    public static class SizeExtensions
    {
        public static int Area(this Size size) =>
            size.Height * size.Width;
        
        public static Size HeightSize(this Size size) =>
            new Size(0,size.Height);

        public static Size WidthSize(this Size size) =>
            new Size(0, size.Width);

        public static Size Divide(this Size size, int by) =>
            new Size(size.Width /= by, size.Height /= by);

        public static Rectangle WithCenterIn(this Size size, Point center) =>
            new Rectangle(center - size.Divide(2), size);
    }
}