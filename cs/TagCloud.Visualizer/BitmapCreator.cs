﻿using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagCloud.Visualizer
{
    public static class BitmapCreator
    {
        internal static void DrawAndSaveBitmap(List<Rectangle> rectangles, string name, string path)
        {
            var bitmap = CreateBitmap(rectangles);
            var graph = Graphics.FromImage(bitmap);
            foreach (var rect in rectangles)
            {
                graph.FillRectangle(Brushes.Chocolate, rect);
                graph.DrawRectangle(Pens.Aqua, rect);
            }
            bitmap.Save(Path.Combine(path, $"{name}.bmp"));
        }

        private static Bitmap CreateBitmap(IReadOnlyCollection<Rectangle> rectangles)
        {
            var width = rectangles.OrderByDescending(rect => rect.Right).First().Right;
            var height = rectangles.OrderByDescending(rect => rect.Bottom).First().Bottom;
            return new Bitmap(width, height);
        }
    }
}