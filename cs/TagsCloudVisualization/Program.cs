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

            var bitmap = GenerateRandomCloudBitmap(center, imageSize, rnd.Next(80, 150),
                25, 75, 25, 75);
            bitmap.Save($"1_RandomLayout.bmp");
            bitmap = GenerateRandomCloudBitmap(center, imageSize, rnd.Next(80, 150),
                25, 25, 20, 50);
            bitmap.Save($"2_Strecthed_rectangles.bmp");
            bitmap = GenerateRandomCloudBitmap(center, imageSize, rnd.Next(80, 150),
                50, 50, 50, 50);
            bitmap.Save($"3_Squares.bmp");
        }

        public static Bitmap GenerateRandomCloudBitmap(Point center, Size imageSize, int amount,
            int minWidth, int maxWidth, int minHeight, int maxHeight)
        {
            var rectangles = CloudLayouterUtilities.GenerateRandomLayout(center, amount,
                minWidth, maxWidth, minHeight, maxHeight);

            return CloudLayouterUtilities.GetBitmapFromRectangles(imageSize, rectangles);
        }
    }
}
