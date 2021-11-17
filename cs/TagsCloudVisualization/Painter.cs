using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Painter
    {
        public static void Paint(IReadOnlyList<Rectangle> rectangles,
            string imageName = "image",
            string path = null)
        {
            path ??= @"..\..\..\Tests\Tests_Images\";
            var imageSize = GeometryHelper.GetRectanglesCommonSize(rectangles);
            var imageCenter = new Point(imageSize.Width / 2, imageSize.Height / 2);

            using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.White);

            graphics.TranslateTransform(imageCenter.X, imageCenter.Y);


            graphics.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save($@"{path}{imageName}.bmp", ImageFormat.Bmp);
        }
    }
}