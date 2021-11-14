using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloudVisualisation
{
    public static class BitmapSaver
    {
        public static void SaveRectangleRainbowBitmap(IEnumerable<Rectangle> rectangles, string path)
        {
            var random = new Random();
            var bitmap = new Bitmap(1000, 1000);
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rectangle in rectangles)
            {
                var r = random.Next(80, 255);
                var g = random.Next(80, 255);
                var b = random.Next(80, 255);
                graphics.DrawRectangle(new Pen(Color.FromArgb(r, g, b)), rectangle);
            }

            bitmap.Save(path);
        }
    }
}
