using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Painter
    {
        public static void Paint(IReadOnlyList<Rectangle> rectangles, string imageName = "image")
        {
            var imageSize = GeometryHelper.GetRectanglesCommonSize(rectangles);
            var imageCentre = new Point(imageSize.Width / 2, imageSize.Height / 2);

            using var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            using var graphics = Graphics.FromImage(bitmap);
            using var pen = new Pen(Color.White);
            
            graphics.TranslateTransform(imageCentre.X, imageCentre.Y);


            graphics.DrawRectangles(pen, rectangles.ToArray());
            bitmap.Save($@"..\..\..\Tests\Tests_Images\{imageName}.bmp", ImageFormat.Bmp);
        }
    }
}