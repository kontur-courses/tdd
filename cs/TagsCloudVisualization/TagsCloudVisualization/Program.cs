using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layout1 = new CircularCloudLayouter(new Point(0, 0));
            var layout2 = new CircularCloudLayouter(new Point(80, 30));
            var layout3 = new CircularCloudLayouter(new Point(-200, 420));

            DrawRandom(layout1, 10, (100, 100), (500, 500), "0x0_15_100x100_500x500.png");
            DrawRandom(layout2, 500, (100, 100), (500, 100), "80x30_500_100x100_500x100.png");
            DrawRandom(layout3, 250, (100, 100), (500, 100), "-200x420_250_100x100_500x100.png");
        }

        private static void DrawRandom(
            CircularCloudLayouter layouter,
            int rectangleCount,
            (int width, int height) minSize,
            (int width, int height) maxSize,
            string fileName)
        {
            var random = new Random();
            var visualizer = new CircularCloudVisualizer();

            for (var i = 0; i < rectangleCount; i++)
                layouter.PutNextRectangle(
                    new Size(
                        random.Next(minSize.width, maxSize.width),
                        random.Next(minSize.height, maxSize.height)));
            var bmp = visualizer.DrawRectangles(layouter.Rectangles, layouter.Radius);
            bmp.Save(fileName);
        }
    }
}