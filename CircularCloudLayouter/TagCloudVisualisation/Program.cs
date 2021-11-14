using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloudVisualisation
{ 
    public class Program
    {
        public static void Main()
        {
            var folder = "RenderedPictures";
            System.IO.Directory.CreateDirectory(folder);
            GenerateFirstPictureAndOpen(folder);
            GenerateSecondPictureAndOpen(folder);
            GenerateThirdPictureAndOpen(folder);
        }

        private static void GenerateFirstPictureAndOpen(string folder)
        {
            var random = new Random();
            var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(500, 500)));
            for (int i = 0; i < 50; i++)
            {
                CCL.PutNewRectangle(new Size(random.Next(1, 100), random.Next(1, 100)));
            }

            BitmapSaver.SaveRectangleRainbowBitmap(CCL.GetRectangles(), folder + @"\1.bmp");
        }

        private static void GenerateSecondPictureAndOpen(string folder)
        {
            var random = new Random();
            var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(500, 500)));
            for (int i = 0; i < 200; i++)
            {
                CCL.PutNewRectangle(new Size(random.Next(1, 20), random.Next(1, 20)));
            }

            BitmapSaver.SaveRectangleRainbowBitmap(CCL.GetRectangles(), folder + @"\2.bmp");
        }

        private static void GenerateThirdPictureAndOpen(string folder)
        {
            var random = new Random();
            var CCL = new CircularCloudLayouter(new ArchimedeanSpiral(new Point(500, 500)));
            for (int i = 0; i < 3000; i++)
            {
                CCL.PutNewRectangle(new Size(random.Next(2, 10), random.Next(4, 40)));
            }

            BitmapSaver.SaveRectangleRainbowBitmap(CCL.GetRectangles(), folder + @"\3.bmp");
        }
    }
}
