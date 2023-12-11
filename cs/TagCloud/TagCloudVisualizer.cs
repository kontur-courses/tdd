using System.Drawing;

namespace TagCloud
{
    public static class TagCloudVisualizer
    {
        public static Bitmap DrawImage(Rectangle[] rectangles, Size imgSize)
        {
            var c1 = Color.FromArgb(255, 90, 137, 185);
            var c2 = Color.FromArgb(255, 10, 28, 52);
            var c3 = Color.FromArgb(255, 193, 217, 249);

            var bmp = new Bitmap(imgSize.Width, imgSize.Height);
            var g = Graphics.FromImage(bmp);
            var pen = new Pen(c2, 5);
            var brush = new SolidBrush(c1);

            g.Clear(c3);

            g.FillRectangles(brush, rectangles);
            g.DrawRectangles(pen, rectangles);
 

            return bmp;
        }
    }
}
