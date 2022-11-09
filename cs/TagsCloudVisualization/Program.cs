using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudVisualization.Curves;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Savers;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Point center = new Point(500, 500);
            ArchimedeanSpiral spiral = new ArchimedeanSpiral( 0, 2.5);
            CircularCloudLayouter layouter = new CircularCloudLayouter(spiral, center);
            int amountOfRectangles = 100;

            Random random = new Random(1);
            for (int i = 0; i < amountOfRectangles; i++)
            {
                int randomScale = random.Next() % 3 + 1;
                Size size = new Size(100 * (random.Next() % 3 + 1), 
                    100 * (random.Next() % 3 + 1));
                layouter.PutNextRectangle(size);
            }

            CircularCloudDrawer drawer = new CircularCloudDrawer(layouter);
            Bitmap image = drawer.CreateImage();
            IBitmapSaver saver = new HardDriveSaver();
            saver.Save(image, "image");
        }
    }
}