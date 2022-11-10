using System;
using System.Drawing;
using TagsCloudVisualization.Curves;
using TagsCloudVisualization.Savers;

namespace TagsCloudVisualization
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            var center = new Point(500, 500);
            var spiral = new ArchimedeanSpiral(0, 2.5);
            var layouter = new CircularCloudLayouter(spiral, center);
            const int amountOfRectangles = 100;

            var random = new Random(1);
            for (var i = 0; i < amountOfRectangles; i++)
            {
                var size = new Size(100 * (random.Next() % 3 + 1),
                    100 * (random.Next() % 3 + 1));
                layouter.PutNextRectangle(size);
            }

            var drawer = new CircularCloudDrawer(layouter);
            var image = drawer.CreateImage();
            IBitmapSaver saver = new HardDriveSaver();
            saver.Save(image, "image");
        }
    }
}