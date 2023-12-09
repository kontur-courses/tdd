using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public static class TagCloudSaver
    {
        private const int BOUND_LENGTH = 10;
        public static void SaveAsPng(List<Rectangle> rectangles, string fileName)
        {
            var width = 0;
            var height = 0;

            foreach (var rectangle in rectangles)
            {
                width = Math.Max(width, rectangle.Right);
                height = Math.Max(height, rectangle.Bottom);
            }

            using var bitmap = new Bitmap(width + BOUND_LENGTH, height + BOUND_LENGTH);
            using var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.AliceBlue);
            var rnd = new Random();
            foreach (var rectangle in rectangles)
            {
                pen.Color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                graphics.DrawRectangle(pen, rectangle);
            }

            var directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            bitmap.Save(fileName, ImageFormat.Png);
        }
    }
}
