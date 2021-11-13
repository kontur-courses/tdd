using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectanglePainter
    {
        public static void SaveToFile(string filePath, IReadOnlyCollection<Rectangle> rectangles)
        {
            var bitmapParams = CalculateBitmapParams(rectangles);
            using var bitmap = new Bitmap(bitmapParams.Width, bitmapParams.Height);
            DrawRectangles(bitmap, rectangles, bitmapParams.DrawingOffset);
            bitmap.Save(filePath, ImageFormat.Png);
        }

        private static void DrawRectangles(Image bitmap, IEnumerable<Rectangle> rectangles, Point offset)
        {
            var random = new Random();
            using var graphics = Graphics.FromImage(bitmap);

            graphics.Clear(Color.Black);
            foreach (var rectangle in rectangles)
            {
                var brush = new SolidBrush(
                    Color.FromArgb(
                        random.Next(50, 255),
                        random.Next(0, 255),
                        random.Next(0, 255)
                    )
                );
                rectangle.Offset(offset);
                graphics.FillRectangle(brush, rectangle);
            }
        }

        private static BitmapParams CalculateBitmapParams(IReadOnlyCollection<Rectangle> rectangles)
        {
            var width = 2 * rectangles.Max(
                x => Math.Max(Math.Abs(x.Right), Math.Abs(x.Left))
            );
            var height = 2 * rectangles.Max(
                x => Math.Max(Math.Abs(x.Top), Math.Abs(x.Bottom))
            );

            return new BitmapParams(width, height, new Point(width / 2, height / 2));
        }
    }

    public readonly struct BitmapParams
    {
        public readonly int Width;
        public readonly int Height;
        public readonly Point DrawingOffset;

        public BitmapParams(int width, int height, Point drawingOffset)
        {
            Width = width;
            Height = height;
            DrawingOffset = drawingOffset;
        }
    }
}