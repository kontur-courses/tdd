using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var layouter = new CircularCloudLayouter(new Point(5000, 5000));
            var visualizer = new Visualizer();
            var rectangles = GenerateRectangles(layouter);

            visualizer.Visualize(rectangles, new Size(10000, 10000))
                .Save("Cloud.png", ImageFormat.Png);
        }

        private static List<Rectangle> GenerateRectangles(CircularCloudLayouter layouter)
        {
            var rnd = new Random();
            for (int i = 0; i < 10000; ++i)
            {
                var size = new Size(rnd.Next(10, 100), rnd.Next(10, 100));
                layouter.PutNextRectangle(size);
            }
            return layouter.GetRectangles();
        }
    }
}
