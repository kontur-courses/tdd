using System;
using System.Drawing;
using System.IO;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectDrawer
    {
        public static string DrawRectangles(Point center, Rectangle[] rectangles)
        {
            var name = FindFreeFileName(Constants.PngExtension);

            return TrySaveRectImages(center, rectangles, name) ? name : null;
        }

        private static string FindFreeFileName(string extension)
        {
            string name;
            do
            {
                name = Guid.NewGuid().ToString();
            } while (File.Exists($"{name}.{extension}"));

            return $"{name}.{extension}";
        }

        private static bool TrySaveRectImages(Point center, Rectangle[] rectangles, string path)
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
            center.Offset(offset);

            var bitmap = new Bitmap(width, height);
            var random = new Random();
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, width, height);
                foreach (var rectangle in rectangles)
                {
                    graphics.FillRectangle(new SolidBrush(GetRandomColor(random)), rectangle.Moved(offset));
                }

                center.Offset(-5, -5);
                graphics.FillEllipse(Brushes.Red, new Rectangle(center, new Size(10, 10)));
            }

            bitmap.Save(path);

            return true;
        }

        private static Color GetRandomColor(Random random)
        {
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }
    }
}
