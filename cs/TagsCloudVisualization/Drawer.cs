using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static void DrawRectangles(IEnumerable<Rectangle> rectangles, string path)
        {
            using var image = new Bitmap(700, 700);
            using var graph = Graphics.FromImage(image);
            var center = rectangles.First().Location;
            graph.TranslateTransform(image.Width / 2 - center.X, image.Height / 2 - center.Y);

            var random = new Random();
            foreach (var rectangle in rectangles)
            {
                var pen = new Pen(GetRandomColor(random), 1);
                graph.DrawRectangle(pen, rectangle);
            }

            image.Save(path);
        }

        private static Color GetRandomColor(Random random) =>
            Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }
}