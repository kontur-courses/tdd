using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TagCloud.Visualizer
{
    internal static class CircularCloudVisualizer
    {
        private static void Main()
        {
            for (var i = 1; i <= 3; i++)
            {
                var rectangles = CreateRandomRectangles(i * 20);
                var dir = Directory.GetCurrentDirectory();
                var path = dir.Remove(dir.IndexOf(@"\bin", StringComparison.Ordinal));
                DrawAndSaveBitmap(rectangles, i.ToString(), path);
            }
        }

        internal static void DrawAndSaveBitmap(IEnumerable<Rectangle> rectangles, string i, string path)
        {
            var bitmap = new Bitmap(1200, 1200);
            var graph = Graphics.FromImage(bitmap);
            foreach (var rect in rectangles)
            {
                graph.FillRectangle(Brushes.Chocolate, rect);
                graph.DrawRectangle(Pens.Aqua, rect);
            }

            SaveBmpToProjectDirectory(i, bitmap, path);
        }

        private static void SaveBmpToProjectDirectory(string name, Bitmap bitmap, string path)
        {
            using var fileStream = File.Create(path + $@"\{name}.bmp");
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