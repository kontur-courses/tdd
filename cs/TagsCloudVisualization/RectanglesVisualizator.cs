using System;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class RectanglesVisualizator
    {
        private const int frameWidth = 100;

        public static bool TryDrawRectanglesAndSaveAsPng(Rectangle[] rectangles, string pathToSave, out string savedPath)
        {
            if (rectangles == null || pathToSave == null)
                throw new ArgumentNullException();

            var imgSize = GetImageSize(rectangles);
            var result = true;
            using (var bm = new Bitmap(imgSize.Width, imgSize.Height))
            {
                var gr = Graphics.FromImage(bm);
                gr.Clear(Color.Black);
                DrawRectangles(GetCenteredRectangles(rectangles), gr);
                savedPath = pathToSave.EndsWith(".png") ? pathToSave : pathToSave + ".png";
                try
                {
                    bm.Save(savedPath, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    result = false;
                }
            }
            return result;
        }

        private static Rectangle[] GetCenteredRectangles(Rectangle[] rectangles)
        {
            if (rectangles.Length == 0)
                return rectangles;

            var minX = rectangles.Min(r => r.X);
            var minY = rectangles.Min(r => r.Y);

            return rectangles
                .Select(r => new Rectangle(
                    new Point(r.X - minX + frameWidth, r.Y - minY + frameWidth),
                    r.Size))
                .ToArray();
        }

        private static Size GetImageSize(Rectangle[] rectangles)
        {
            var (lengthByX, lengthByY) = GetMaxLengthsByXAndByY(rectangles);
            return new Size(2 * frameWidth + lengthByX, 2 * frameWidth + lengthByY);
        }

        private static (int lengthByX, int lengthByY) GetMaxLengthsByXAndByY(Rectangle[] rectangles)
        {
            if (rectangles.Length == 0)
                return (0, 0);

            var minY = rectangles.Min(r => r.Top);
            var minX = rectangles.Min(r => r.Left);
            var maxY = rectangles.Max(r => r.Bottom);
            var maxX = rectangles.Max(r => r.Right);

            var lengthByX = maxX - minX + 1;
            var lengthByY = maxY - minY + 1;

            return (lengthByX, lengthByY);
        }

        private static void DrawRectangles(Rectangle[] rectangles, Graphics graphics)
        {
            var solidBrushRed = new SolidBrush(Color.Red);
            var solidBrushCyan = new SolidBrush(Color.Cyan);
            var penDarkRed = new Pen(Color.DarkRed);
            for (var i = 0; i < rectangles.Length; i++)
            {
                var brush = solidBrushCyan;
                if (i == 0)
                    brush = solidBrushRed;
                graphics.FillRectangle(brush, rectangles[i]);
                graphics.DrawRectangle(penDarkRed, rectangles[i]);
            }
        }
    }
}