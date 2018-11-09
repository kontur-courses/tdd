using System;
using System.Drawing;
using TagsCloudVisualization.Extensions;
using TagsCloudVisualization.Geom;

namespace TagsCloudVisualization
{
    public class EntryPoint
    {
        private const int ImageWidth = 1024;
        private const int ImageHeight = 1024;


        private static void Main(string[] args)
        {
            var randomLayouter = LayouterWithRandomSizeRectangles();
            Console.WriteLine("Random generated");
            var simpleLayouter = SimpleLayouter();
            Console.WriteLine("Simple generated");

            GenerateImage(randomLayouter, "random.png");;
            GenerateImage(simpleLayouter, "simple.png");
        }

        private static CircularCloudLayouter LayouterWithRandomSizeRectangles()
        {
            var layouter = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));
            for (var i = 0; i < 300; i++)
                layouter.PutNextRectangle(new Size().SetRandom(3, 100, 3, 80));

            return layouter;
        }

        private static CircularCloudLayouter SimpleLayouter()
        {
            var layouter = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));
            for (var i = 0; i < 1000; i++)
                layouter.PutNextRectangle(new Size(20, 10));

            return layouter;
        }

        private static void GenerateImage(CircularCloudLayouter layouter, string imageName)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);

            foreach (var r in layouter.Rectangles)
            {
                graphics.FillRectangle(Brushes.LightCoral, r);
                graphics.DrawRectangle(Pens.Black, r);
            }

            bitmap.Save(imageName);
        }
    }
}