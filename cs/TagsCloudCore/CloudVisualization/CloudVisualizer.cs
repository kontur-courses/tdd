using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudCore.CloudVisualization
{
    public static class CloudVisualizer
    {
        public static void SaveLayoutToFile(string filePath, IEnumerable<Rectangle> rectangles, int padding = 50)
        {
            var currentRectangles = rectangles.ToArray();

            if (currentRectangles.Length == 0)
            {
                using var emptyBitmap = new Bitmap(300, 300);
                using var g = Graphics.FromImage(emptyBitmap);

                g.Clear(Color.Black);
                emptyBitmap.Save(filePath, ImageFormat.Png);
                return;
            }

            var firstRectangle = currentRectangles[0];

            var minX = firstRectangle.Left;
            var maxX = firstRectangle.Right;
            var minY = firstRectangle.Top;
            var maxY = firstRectangle.Bottom;

            foreach (var r in currentRectangles)
            {
                if (r.Left < minX) minX = r.Left;
                if (r.Right > maxX) maxX = r.Right;
                if (r.Top < minY) minY = r.Top;
                if (r.Bottom > maxY) maxY = r.Bottom;
            }

            var width = (maxX - minX) + padding * 2;
            var height = (maxY - minY) + padding * 2;

            width = Math.Min(width, 5000);
            height = Math.Min(height, 5000);

            using var bitmap = new Bitmap(width, height);
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);

            using var pen = new Pen(Color.White, 1);
            using var brush = new SolidBrush(Color.White);

            var random = new Random();

            foreach (var rectangle in currentRectangles)
            {
                var shifted = rectangle with
                {
                    X = rectangle.Left - minX + padding, Y = rectangle.Top - minY + padding
                };

                brush.Color = Color.FromArgb(
                    255,
                    random.Next(40, 255),
                    random.Next(40, 255),
                    random.Next(40, 255));

                graphics.FillRectangle(brush, shifted);
                graphics.DrawRectangle(pen, shifted);
            }

            bitmap.Save(filePath, ImageFormat.Png);
        }
    }
}