using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private Point rectanglesCenter;
        private int maxDistanceToCenter;

        public Bitmap Visualize(IEnumerable<Rectangle> rectangles, Point center)
        {
            rectanglesCenter = center;
            var rects = rectangles as Rectangle[] ?? rectangles.ToArray();
            SetMaxDistanceFromCenter(rects);
            var size = GetBitmapSize(rects);
            var bitmap = new Bitmap(size, size);
            var graphics = Graphics.FromImage(bitmap);
            graphics.DrawRectangle(new Pen(Color.Blue), new Rectangle(new Point(0, 0), new Size(size - 1, size - 1)));
            graphics.TranslateTransform(bitmap.Width / 2, bitmap.Height / 2);
            foreach (var rect in rects)
            {
                var color = GetRectangleColor(rect);
                graphics.FillRectangle(new SolidBrush(color), rect);
            }
            return bitmap;
        }

        private int GetBitmapSize(IEnumerable<Rectangle> rectangles)
        {
            var xCoordinates = rectangles.Select(rectangle => rectangle.Location.X).ToArray();
            var maxX = xCoordinates.Max();
            var minX = xCoordinates.Min();
            var size = maxX - minX + Math.Max(rectanglesCenter.X, rectanglesCenter.Y) * 2 + 50;
            return size;
        }

        private void SetMaxDistanceFromCenter(IEnumerable<Rectangle> rectangles)
        {
            maxDistanceToCenter = (int)rectangles.Select(rect => rect.Location.DistanceTo(rectanglesCenter)).Max();
        }

        private Color GetRectangleColor(Rectangle rectangle)
        {
            var distanceToCenter = rectangle.Location.DistanceTo(rectanglesCenter);
            var red = (int)(distanceToCenter / maxDistanceToCenter * 255 * 0.8);
            var green = (int)(distanceToCenter / maxDistanceToCenter * 255 * 0.4);
            var blue = (int)(distanceToCenter / maxDistanceToCenter * 255 * 0.9);
            return Color.FromArgb(red, green, blue);
        }
    }
}