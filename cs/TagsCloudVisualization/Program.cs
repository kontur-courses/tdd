using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public const int ImageWidth = 1024;
        public const int ImageHeight = 1024;


        public static void Main(string[] args)
        {
            var randomLayouter = LayouterWithRandomSizeRectangles();
            Console.WriteLine("Random generated");
            var simpleLayouter = SimpleLayouter();
            Console.WriteLine("Simple generated");

            GenerateImage(randomLayouter, "random.png");;
            GenerateImage(simpleLayouter, "simple.png");
        }

        public static CircularCloudLayouter LayouterWithRandomSizeRectangles()
        {
            var layouter = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));
            for (var i = 0; i < 300; i++)
                layouter.PutNextRectangle(new Size().SetRandom(100, 80));

            return layouter;
        }

        public static CircularCloudLayouter SimpleLayouter()
        {
            var layouter = new CircularCloudLayouter(new Point(ImageWidth / 2, ImageHeight / 2));
            for (var i = 0; i < 1000; i++)
                layouter.PutNextRectangle(new Size(20, 10));

            return layouter;
        }

        public static void GenerateImage(CircularCloudLayouter layouter, string imageName)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var gr = Graphics.FromImage(bitmap);

            foreach (var r in layouter.Rectangles)
            {
                gr.FillRectangle(Brushes.LightCoral, r);
                gr.DrawRectangle(Pens.Black, r);
            }

            bitmap.Save(imageName);
        }
    }
}