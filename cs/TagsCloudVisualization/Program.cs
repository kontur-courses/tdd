using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public static class Program
    {
        public static Random rnd = new Random();

        public static void Main()
        {
            var center = new Point(800, 600);
            var imageSize = new Size(1600, 1200);

            var bitmap = GenerateCloudBitmap(center, imageSize, rnd.Next(80, 150),
                25, 75, 25, 75);
            bitmap.Save($"1_RandomLayout.bmp");
            bitmap = GenerateCloudBitmap(center, imageSize, rnd.Next(80, 150),
                25, 25, 20, 50);
            bitmap.Save($"2_Strecthed_rectangles.bmp");
            bitmap = GenerateCloudBitmap(center, imageSize, rnd.Next(80, 150),
                50, 50, 50, 50);
            bitmap.Save($"3_Squares.bmp");
        }

        public static Bitmap GenerateCloudBitmap(Point center, Size imageSize, int amount, 
            int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);

            var rectangles = RandomLayoutGenerator.GenerateRandomLayout(center, amount,
                minWidth, maxWidth, minHeight, maxHeight);

            foreach (var rect in rectangles)
            {
                var brush = new SolidBrush(
                    Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256)));
                graphics.FillRectangle(brush, rect);
            }

            return bitmap;
        }
    }
}
