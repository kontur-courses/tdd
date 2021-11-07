using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class LayoutImageGenerator
    {
        public static string CreateImage(IReadOnlyCollection<Rectangle> rectangles, Size imageSize, string path)
        {
            if (!rectangles.Any())
                throw new ArgumentException("No rectangles received");
            var bm = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bm);

            graphics.DrawRectangles(new Pen(Color.DarkOrange, 4), rectangles.ToArray());

            var savePath = $"{path}\\layout_{DateTime.Now:HHmmssddMM}.bmp";
            bm.Save($"{path}\\layout_{DateTime.Now:HHmmssddMM}.bmp", ImageFormat.Bmp);
            return savePath;
        }

        public static string CreateImage(CircularCloudLayouter layouter)
        {
            var imageSize = new Size(layouter.Center) * 2;
            return CreateImage(layouter.Rectangles, imageSize, AppDomain.CurrentDomain.BaseDirectory);
        }

        public static string GenerateImage()
        {
            var layouter = CircularCloudLayouter.Generate(new Point(500, 500), 100);
            var imageSize = new Size(layouter.Center) * 2;

            return CreateImage(layouter.Rectangles, imageSize, $"{AppDomain.CurrentDomain.BaseDirectory}");
        }
    }
}
