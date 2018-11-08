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
            var layout3 = new CircularCloudLayouter(new Point(-13, 42));

            DrawRandom(layout1, 10, (100, 100), (500, 500), "0x0_15_100x100_500x500.bmp");
            DrawRandom(layout2, 15, (100, 100), (500, 100), "80x30_10_100x100_500x100.bmp");
            DrawRandom(layout3, 25, (100, 100), (500, 100), "-13x42_25_100x100_500x100.bmp");
        }

        private static void DrawRandom(
            CircularCloudLayouter layouter,
            int randomCount,
            (int width, int height) minSize,
            (int width, int height) maxSize,
            string fileName)
        {
            var random = new Random();
            var visualizer = new CircularCloudVisualizer();

            for (var i = 0; i < randomCount; i++)
            {
                layouter.PutNextRectangle(
                    new Size(
                        random.Next(minSize.width, maxSize.width),
                        random.Next(minSize.height, maxSize.height)));
                var bmp = visualizer.DrawRectangles(layouter.Rectangles);
                bmp.Save(fileName);
            }
        }
    }
}