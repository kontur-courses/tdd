using System;
using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization
{
    public class EntryPoint
    {
        private const int DefaultImageWidth = 1024;
        private const int DefaultImageHeight = 1024;

        private static void Main(string[] args)
        {
            var randomLayouter = LayouterWithRandomSizeRectangles();
            Console.WriteLine("Random generated");
            var simpleLayouter = SimpleLayouter();
            Console.WriteLine("Simple generated");

            GenerateImage(randomLayouter, "random.png"); ;
            GenerateImage(simpleLayouter, "simple.png");
        }

        private static CircularCloudLayouter LayouterWithRandomSizeRectangles()
        {
            var layouter = new CircularCloudLayouter(DefaultImageWidth / 2, DefaultImageHeight / 2, DefaultImageWidth, DefaultImageHeight);
            for (var i = 0; i < 300; i++)
                layouter.PutNextRectangle(new Size().SetRandom(3, 100, 3, 80));

            return layouter;
        }

        private static CircularCloudLayouter SimpleLayouter()
        {
            var layouter = new CircularCloudLayouter(DefaultImageWidth / 2, DefaultImageHeight / 2, DefaultImageWidth, DefaultImageHeight);
            for (var i = 0; i < 1000; i++)
                layouter.PutNextRectangle(new Size(20, 10));

            return layouter;
        }

        private static void GenerateImage(CircularCloudLayouter layouter, string imageName)
        {
            var bitmap = new Bitmap(DefaultImageWidth, DefaultImageHeight);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var r in layouter.Rectangles)
            {
                graphics.FillRectangle(Brushes.LightCoral, r);
                graphics.DrawRectangle(Pens.Black, r);
            }

            bitmap.Save(imageName);
        }

        private static void GenerateImage(Spiral spiral, string imageName)
        {
            var bitmap = new Bitmap(DefaultImageWidth, DefaultImageHeight);
            var graphics = Graphics.FromImage(bitmap);

            for (var i = 0; i < 400000; i++)
            {
                var point = spiral.GetNextLocation();
                graphics.FillRectangle(Brushes.Black, new RectangleF(point, new SizeF(1, 1)));
            }

            bitmap.Save(imageName);
        }
    }
}