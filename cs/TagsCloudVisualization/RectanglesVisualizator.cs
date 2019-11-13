using System;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public static class RectanglesVisualizator
    {
        private const int frameWidth = 100;

        public static bool TryDrawRectanglesAndSaveAsPng(List<Rectangle> rectangles, string pathToSave, out string savedPath)
        {
            if (rectangles == null || pathToSave == null)
                throw new ArgumentNullException();

            var imgSize = GetImageSize(rectangles);
            using (var bm = new Bitmap(imgSize.Width, imgSize.Height))
            {
                using (var gr = Graphics.FromImage(bm))
                {
                    gr.Clear(Color.Black);
                    DrawRectangles(GetCenteredRectangles(rectangles), gr);
                }
                savedPath = pathToSave.EndsWith(".png") ? pathToSave : pathToSave + ".png";
                try
                {
                    bm.Save(savedPath, System.Drawing.Imaging.ImageFormat.Png);
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        private static List<Rectangle> GetCenteredRectangles(List<Rectangle> rectangles)
        {
            if (rectangles.Count == 0)
                return rectangles;

            var minX = rectangles.Min(r => r.X);
            var minY = rectangles.Min(r => r.Y);

            return rectangles
                .Select(r => new Rectangle(
                    new Point(r.X - minX + frameWidth, r.Y - minY + frameWidth),
                    r.Size))
                .ToList();
        }

        private static Size GetImageSize(List<Rectangle> rectangles)
        {
            var (lengthByX, lengthByY) = GetMaxLengthsByXAndByY(rectangles);
            return new Size(2 * frameWidth + lengthByX, 2 * frameWidth + lengthByY);
        }

        private static (int lengthByX, int lengthByY) GetMaxLengthsByXAndByY(List<Rectangle> rectangles)
        {
            if (rectangles.Count == 0)
                return (0, 0);

            var minY = rectangles.Min(r => r.Top);
            var minX = rectangles.Min(r => r.Left);
            var maxY = rectangles.Max(r => r.Bottom);
            var maxX = rectangles.Max(r => r.Right);

            var lengthByX = maxX - minX + 1;
            var lengthByY = maxY - minY + 1;

            return (lengthByX, lengthByY);
        }

        private static void DrawRectangles(List<Rectangle> rectangles, Graphics graphics)
        {
            var penDarkRed = new Pen(Color.DarkRed);
            DrawRectangles(rectangles.Take(1), graphics, new SolidBrush(Color.Red), penDarkRed);
            DrawRectangles(rectangles.Skip(1), graphics, new SolidBrush(Color.Cyan), penDarkRed);
        }

        private static void DrawRectangles(IEnumerable<Rectangle> rectangles, Graphics graphics, Brush brush, Pen pen)
        {
            foreach (var rectangle in rectangles)
            {
                graphics.FillRectangle(brush, rectangle);
                graphics.DrawRectangle(pen, rectangle);
            }
        }
    }
}