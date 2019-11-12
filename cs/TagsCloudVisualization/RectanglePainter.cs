using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public static class RectanglePainter
    {
        public static Bitmap GetImageOfRectangles(List<Rectangle> rectangles, int widthOfBorder=0)
        {
            var borderOfRectangles = GetBorderOfRectangles(rectangles);
            var image = new Bitmap(borderOfRectangles.Width + widthOfBorder * 2, borderOfRectangles.Height + widthOfBorder * 2);
            var canvas = Graphics.FromImage(image);
            canvas.Clear(Color.White);
            var rand = new Random();
            foreach (var rectangle in rectangles)
            {
                var rect = rectangle;
                var r = rand.Next(256);
                var g = rand.Next(256);
                var b = rand.Next(256);
                canvas.FillRectangle(new SolidBrush(Color.FromArgb(r, g,b)), new Rectangle(rect.X - borderOfRectangles.X + widthOfBorder, 
                    rect.Y - borderOfRectangles.Y + widthOfBorder, 
                    rect.Width, rect.Height));
            }
            return image;
        }

        private static Rectangle GetBorderOfRectangles(List<Rectangle> rectangles)
        {
            if (rectangles.Count == 0)
                throw new ArgumentException("Value cannot be an empty collection.", nameof(rectangles));
            var maxX = rectangles.Max(rectangle => rectangle.Right);
            var maxY = rectangles.Max(rectangle => rectangle.Bottom);
            var minX = rectangles.Min(rectangle => rectangle.Left);
            var minY = rectangles.Min(rectangle => rectangle.Top);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }
}
