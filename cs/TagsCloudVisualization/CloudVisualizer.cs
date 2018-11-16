using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace TagsCloudVisualization
{
    public class CloudVisualizer : ICloudVisualizer
    {
        public IEnumerable<Rectangle> Rectangles { get; }

        public CloudVisualizer(IEnumerable<Rectangle> rectangles)
        {
            Rectangles = rectangles;
        }

        public Bitmap CreateImage(string path)
        {
            var width = Rectangles.Max(r => r.Right) - Rectangles.Min(r => r.Left);
            var height = Rectangles.Max(r => r.Bottom) - Rectangles.Min(r => r.Top);
            var image = new Bitmap(width * 2, height * 2);
            var graphics = Graphics.FromImage(image);
            var newCenter = new Point(image.Width / 2, image.Height / 2);
            var random = new Random();
            var brushes = typeof(Brushes)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(pi => (Brush)pi.GetValue(null, null))
                .ToList();

            foreach (var rectangle in Rectangles)
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
