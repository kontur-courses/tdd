using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class CloudVisualizer : ICloudVisualizer
    {
        public Bitmap CreateImage(IEnumerable<Rectangle> rectangles, string path)
        {
            rectangles = rectangles.ToList();
            var width = rectangles.Max(r => r.Right) - rectangles.Min(r => r.Left);
            var height = rectangles.Max(r => r.Bottom) - rectangles.Min(r => r.Top);
            var image = new Bitmap(width * 2, height * 2);
            var graphics = Graphics.FromImage(image);
            var newCenter = new Point(image.Width / 2, image.Height / 2);
            var random = new Random();
            var brushes = typeof(Brushes)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(pi => (Brush)pi.GetValue(null, null))
                .ToList();

            foreach (var rectangle in rectangles)
            {
                var movedRectangle = new Rectangle(
                    rectangle.X + newCenter.X, rectangle.Y + newCenter.Y, rectangle.Width, rectangle.Height);
                graphics.FillRectangle(brushes[random.Next(brushes.Count)], movedRectangle);
                graphics.DrawRectangle(Pens.Black, movedRectangle);
            }

            image.Save(path);
            return image;
        }
    }
}
