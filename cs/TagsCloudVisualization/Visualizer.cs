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
            var rectangles = GenerateRandomRectangles(count, scale, out var width, out var height);
            var bitmap = CreateBitmap(rectangles, width, height, $"random-count{count}-scale{scale}", guidingCircle);

            return bitmap;
        }

        private static IEnumerable<Rectangle> GenerateRandomRectangles(int count, double scale, out int width, out int height)
        {
            var layouter = new CircularCloudLayouter(Point.Empty);
            var rnd = new Random();
            var rectangles = new List<Rectangle>(count);
            var maxHeight = 0;
            var minHeight = 0;
            var maxWidth = 0;
            var minWidth = 0;
            for (var i = 0; i < count; i++)
            {
                var size = new Size((int)(rnd.Next(3, 100) * scale), (int)(rnd.Next(5, 20) * scale));
                var rectangle = layouter.PutNextRectangle(size);
                rectangles.Add(rectangle);
                if (rectangle.Top < maxHeight)
                    maxHeight = rectangle.Top;
                if (rectangle.Bottom > minHeight)
                    minHeight = rectangle.Bottom;
                if (rectangle.Right > maxWidth)
                    maxWidth = rectangle.Right;
                if (rectangle.Left < minWidth)
                    minWidth = rectangle.Left;
            }

            width = Math.Abs(minWidth) + Math.Abs(maxWidth) + 100;
            height = Math.Abs(minHeight) + Math.Abs(maxHeight) + 100;
            var offsetX = width / 2;
            var offsetY = height / 2;

            return OffsetRectangles(rectangles.ToArray(), offsetX, offsetY);
        }

        private static Rectangle[] OffsetRectangles(Rectangle[] rectangles, int offsetX, int offsetY)
        {
            for (var i = 0; i < rectangles.Length; i++)
                rectangles[i].Offset(offsetX, offsetY);
            return rectangles;
        }

        private static Bitmap CreateBitmap(IEnumerable<Rectangle> rectangles, int width, int height, string name, bool guidingCircle = false)
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
                    var radius = (int)(height * 0.8);
                    bitmapGraphics.DrawEllipse(Pens.LightGray, (width - radius) / 2, (height - radius) / 2, radius, radius);
                }
            }

            bitmap.Save($"{name}.png", ImageFormat.Png);
            return bitmap;
        }
    }
}
