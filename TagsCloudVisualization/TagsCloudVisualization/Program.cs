using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    class Program
    {
        static void Main(string[] args)
        {
            var rnd = new Random();
            var rectangles = new List<Rectangle>();
            var layouter = new CircularCloudLayouter(new Point(500, 500));
            for (int i = 1; i <= 100; i++)
                rectangles.Add(layouter.PutNextRectangle(new Size(10 + rnd.Next(100),
                    10 + rnd.Next(100))));

            TagCloudVisualizer.Visualize(layouter, new Size(1000, 1000))
                .Save("result.png", ImageFormat.Png);
        }
    }
}
