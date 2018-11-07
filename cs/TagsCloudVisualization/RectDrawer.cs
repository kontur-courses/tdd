using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectDrawer
    {
        public static string DrawRectangles(Rectangle[] rectangles)
        {
            var name = FindFreeFileName(DateTime.Now.ToString("DD-MM-yyyy_HH-mm-ss"), "png");
            
            return TrySaveRectImages(rectangles, name) ? name : null;
        }

        private static string FindFreeFileName(string prefix, string extension)
        {
            var datetimeString = prefix;
            var name = datetimeString;
            var i = 0;
            while (File.Exists($"{name}.{extension}"))
            {
                name += $"({i})";
                i++;
            }

            return $"{name}.{extension}";
        }

        private static bool TrySaveRectImages(Rectangle[] rectangles, string path)
        {
            if (!rectangles.Any())
            {
                return false;
            }

            var minX = rectangles.Select(r => r.Left).Min();
            var maxX = rectangles.Select(r => r.Right).Max();
            var minY = rectangles.Select(r => r.Top).Min();
            var maxY = rectangles.Select(r => r.Bottom).Max();

            var width = maxX - minX + 1;
            var height = maxY - minY + 1;

            var offset = new Point(-minX, -minY);

            var bitmap = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, 0, 0, width, height);
                g.DrawRectangles(Pens.Red, rectangles.Select(r => r.Moved(offset)).ToArray());
            }

            bitmap.Save(path);

            return true;
        }
    }
}
