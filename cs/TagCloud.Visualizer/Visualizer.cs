using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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

                SaveBmpToProjectDirectory(i, bitmap);
            }
        }

        private static void SaveBmpToProjectDirectory(int i, Bitmap bitmap)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = dir.Remove(dir.IndexOf(@"\bin", StringComparison.Ordinal));
            using var fileStream = File.Create(path + $@"\{i}.bmp");
            bitmap.Save(fileStream, ImageFormat.Bmp);
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