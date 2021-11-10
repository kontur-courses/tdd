using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public static class Visualizer
    {
        public static Bitmap GetRandomBitmap(int count, double scale, bool guidingCircle = true)
        {
            var rectangles = GenerateRandomRectangles(count, scale);
            var (width, height) = FitRectangles(rectangles);
            ReplaceRectangles(rectangles, width + 100, height + 100);

            var bitmap = CreateBitmap(rectangles, width, height, $"random-count{count}-scale{scale}", guidingCircle, true, 100);

            return bitmap;
        }

        private static void ReplaceRectangles(Rectangle[] rectangles, int width, int height)
        {
            var offsetX = width / 2;
            var offsetY = height / 2;
            OffsetRectangles(rectangles, offsetX, offsetY);
        }

        public static Bitmap GetBitmapFromRectangles(Rectangle[] rectangles, bool guidingCircle = false, int padding = 0)
        {
            var (width, height) = FitRectangles(rectangles);
            ReplaceRectangles(rectangles, width + padding, height + padding);

            var bitmap = CreateBitmap(rectangles, width, height, null, guidingCircle, false, padding);

            return bitmap;
        }

        private static (int width, int height) FitRectangles(Rectangle[] rectangles)
        {
            var maxHeight = 0;
            var minHeight = 0;
            var maxWidth = 0;
            var minWidth = 0;
            foreach (var rectangle in rectangles)
            {
                if (rectangle.Top < maxHeight)
                    maxHeight = rectangle.Top;
                if (rectangle.Bottom > minHeight)
                    minHeight = rectangle.Bottom;
                if (rectangle.Right > maxWidth)
                    maxWidth = rectangle.Right;
                if (rectangle.Left < minWidth)
                    minWidth = rectangle.Left;
            }

            var width = Math.Abs(minWidth) + Math.Abs(maxWidth);
            var height = Math.Abs(minHeight) + Math.Abs(maxHeight);

            return (width, height);
        }

        private static Rectangle[] GenerateRandomRectangles(int count, double scale)
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            var rnd = new Random();
            var rectangles = new List<Rectangle>(count);
            for (var i = 0; i < count; i++)
            {
                var size = new Size((int)(rnd.Next(5, 100) * scale), (int)(rnd.Next(3, 30) * scale));
                rectangles.Add(layouter.PutNextRectangle(size));
            }

            return rectangles.ToArray();
        }

        private static void OffsetRectangles(Rectangle[] rectangles, int offsetX, int offsetY)
        {
            for (var i = 0; i < rectangles.Length; i++)
                rectangles[i].Offset(offsetX, offsetY);
        }

        private static Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles, int width, int height, string name,
            bool guidingCircle = false, bool autoSave = false, int padding = 0)
        {
            var bitmap = new Bitmap(width + padding, height + padding);
            using (var bitmapGraphics = Graphics.FromImage(bitmap))
            {
                bitmapGraphics.Clear(Color.Black);
                foreach (var rectangle in rectangles)
                {
                    bitmapGraphics.FillRectangle(Brushes.Red, rectangle);
                    bitmapGraphics.DrawRectangle(Pens.Gray, rectangle);
                }

                if (guidingCircle)
                {
                    var radius = Math.Max(height, width);
                    bitmapGraphics.DrawEllipse(Pens.LightGray, (width - radius + padding) / 2, (height - radius + padding) / 2, radius, radius);
                }
            }

            if (autoSave)
                bitmap.Save($"{name}.png", ImageFormat.Png);
            return bitmap;
        }
    }
}
