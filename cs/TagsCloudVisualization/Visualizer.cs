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
            ReplaceRectangles(rectangles, width, height);

            var bitmap = CreateBitmap(rectangles, width, height, $"random-count{count}-scale{scale}", guidingCircle, true);

            return bitmap;
        }

        private static void ReplaceRectangles(Rectangle[] rectangles, int width, int height)
        {
            var offsetX = width / 2;
            var offsetY = height / 2;
            OffsetRectangles(rectangles, offsetX, offsetY);
        }

        public static Bitmap GetBitmapFromRectangles(Rectangle[] rectangles, bool guidingCircle = false)
        {
            var (width, height) = FitRectangles(rectangles);
            ReplaceRectangles(rectangles, width, height);

            var bitmap = CreateBitmap(rectangles, width, height, null, guidingCircle, false);

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

            var width = Math.Abs(minWidth) + Math.Abs(maxWidth) + 100;
            var height = Math.Abs(minHeight) + Math.Abs(maxHeight) + 100;

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
            bool guidingCircle = false, bool autoSave = false)
        {
            var bitmap = new Bitmap(width, height);
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
                    var radius = height - 100;
                    bitmapGraphics.DrawEllipse(Pens.LightGray, (width - radius) / 2, (height - radius) / 2, radius, radius);
                }
            }

            if (autoSave)
                bitmap.Save($"{name}.png", ImageFormat.Png);
            return bitmap;
        }
    }
}
