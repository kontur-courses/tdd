using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Drawer
    {
        public static Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            var image = new Bitmap(700, 700);
            var graph = Graphics.FromImage(image);

            var random = new Random();
            foreach (var rectangle in rectangles)
            {
                var pen = new Pen(GetRandomColor(random), 1);
                graph.DrawRectangle(pen, rectangle);
            }

            return image;
        }

        private static Color GetRandomColor(Random random) =>
            Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
    }
}