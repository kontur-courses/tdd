using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TagCloud
{
    public static class VisualizationPrinter
    {
        public static void Print(string name, IEnumerable<Rectangle> rectangles, Point center)
        {
            var bmp = GetImage(rectangles, center);
            var path = GetPath(name);
            bmp.Save(path);
        }
        
        private static string GetPath(string name, [CallerFilePath] string callerFilePath = "")
        {
            var dir = Path.Combine(Path.GetDirectoryName(callerFilePath), "visualizations");
            Directory.CreateDirectory(dir);
            var filename = $"visualization-{name}.png";
            var path = Path.Combine(dir, filename);
            Console.WriteLine($"Изображение сохранено в {path}");
            return path;
        }
        private static Bitmap GetImage(IEnumerable<Rectangle> rects, Point center)
        {
            var padding = 20;
            var bounds = GetBounds(rects);
            var bmpWidth = Math.Max(100, bounds.Width + padding * 2);
            var bmpHeight = Math.Max(100, bounds.Height + padding * 2);
            var bmp = new Bitmap(bmpWidth, bmpHeight);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            var offsetX = -bounds.Left + padding;
            var offsetY = -bounds.Top + padding;
            var rnd = new Random(0);
            foreach (var r in rects)
            {
                var rect = new Rectangle(r.Left + offsetX, r.Top + offsetY, r.Width, r.Height);
                var brush = new SolidBrush(Color.FromArgb(200, (byte)rnd.Next(60, 230), (byte)rnd.Next(60, 230), (byte)rnd.Next(60, 230)));
                g.FillRectangle(brush, rect);
                g.DrawRectangle(Pens.Black, rect);
            }
            return bmp;
        }


        private static Rectangle GetBounds(IEnumerable<Rectangle> rects)
        {
            var left = rects.Min(r => r.Left);
            var top = rects.Min(r => r.Top);
            var right = rects.Max(r => r.Right);
            var bottom = rects.Max(r => r.Bottom);
            return new Rectangle(left, top, right - left, bottom - top);

        }
    }
}
