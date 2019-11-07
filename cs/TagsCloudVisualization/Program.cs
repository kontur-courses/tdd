using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagsCloudVisualization
{
    public class Program
    {
        private const string ExamplesDirectory = @"\..\..\Examples\";
        private static readonly CircularCloudVisualizer visualizer = new CircularCloudVisualizer();

        public static void Main(string[] args)
        {
            MakeExample1();
            MakeExample2();
            MakeExample3();
            MakeExample4();
        }

        private static CircularCloudLayouter GetLayout(Point center, int count, int startWidth, int startHeight, int step)
        {
            var layouter = new CircularCloudLayouter(center);
            var width = startWidth;
            var height = startHeight;
            for (var i = 0; i < count; i++)
            {
                width += step;
                height += step;
                layouter.PutNextRectangle(new Size(width, height));
            }

            return layouter;
        }

        private static void SaveRectangles(CircularCloudLayouter layouter, string fileName)
        {
            var image = visualizer.VisualizeLayout(layouter);
            var fullPath = Directory.GetCurrentDirectory() + ExamplesDirectory + fileName;
            image.Save(fullPath, ImageFormat.Png);
        }

        private static void MakeExample1()
        {
            var layouter = GetLayout(new Point(1000, 1000), 30, 20, 10, 10);
            SaveRectangles(layouter, "Example1.png");
        }

        private static void MakeExample2()
        {
            var layouter = GetLayout(new Point(1000, 1000), 30, 320, 310, -10);
            SaveRectangles(layouter, "Example2.png");
        }

        private static void MakeExample3()
        {
            var layouter = GetLayout(new Point(500, 500), 200, 30, 20, 0);
            SaveRectangles(layouter, "Example3.png");
        }

        private static void MakeExample4()
        {
            var count = 500;
            var random = new Random();
            var layouter = new CircularCloudLayouter(new Point(1000, 1000));
            for (var i = 0; i < count; i++)
            {
                var width = random.Next(50, 100);
                var height = random.Next(40, 90);
                layouter.PutNextRectangle(new Size(width, height));
            }

            SaveRectangles(layouter, "Example4.png");
        }
    }
}