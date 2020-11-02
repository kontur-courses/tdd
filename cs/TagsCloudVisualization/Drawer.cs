using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace TagsCloudVisualization
{
    class Drawer
    {       
        public static void DrawImage(List<Rectangle> rectangles, Point center, int maxX, int maxY, string fileName)
        {
            var path = Directory.GetCurrentDirectory();

            if (!rectangles.Any())
                throw new ArgumentException();

            using var image = new Bitmap( center.X + maxX, center.Y + maxY);
            using var graphics = Graphics.FromImage(image);

            graphics.DrawRectangles(new Pen(Color.Red), rectangles.ToArray());

            image.Save($"{path}\\{fileName}.bmp", ImageFormat.Bmp);
        }

    }
}
