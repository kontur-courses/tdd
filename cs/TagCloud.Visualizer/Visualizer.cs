using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagCloud.Visualizer
{
    internal static class Visualizer
    {
        private static void Main()
        {
            for (var i = 1; i <= 3; i++)
            {
                var bitmap = new Bitmap(1200, 1200);
                var graph = Graphics.FromImage(bitmap);
                foreach (var rect in CreateRandomRectangles(i * 20))
                {
                    graph.FillRectangle(Brushes.Chocolate, rect);
                    graph.DrawRectangle(Pens.Aqua, rect);
                }

                bitmap.Save($"{i}.bmp");
            }
        }

        private static IEnumerable<Rectangle> CreateRandomRectangles(int amount)
        {
            var rnd = new Random();
            var cloudLayouter = new CircularCloudLayouter(new Point(600, 600));
            for (var counter = 0; counter < amount; counter++)
                yield return cloudLayouter.PutNextRectangle(new Size(rnd.Next(50, 150), rnd.Next(50, 150)));
        }
    }
}