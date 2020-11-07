using System;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace TagsCloudVisualization
{
    public class LayoutDrawer
    {
        private readonly Random random;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public LayoutDrawer(CircularCloudLayouter layouter)
        {
            random = new Random();
            bitmap = new Bitmap(layouter.Center.X * 2, layouter.Center.Y * 2);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                DrawRectangle(rectangle);
        }

        private void DrawRectangle(Rectangle rectangle)
        {
            Brush brush = new SolidBrush(Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255)));
            graphics.FillRectangle(brush, rectangle);
        }

        public void DrawCircle(Point center, int radius)
        {
            var pen = new Pen(Color.Black);
            graphics.DrawEllipse(pen, center.X - radius, center.Y - radius, radius * 2, radius * 2);
        }

        public void Save(string filename, string path = null)
        {
            path ??= Directory.GetCurrentDirectory();
            var filePath = $"{path}\\{filename}.bmp";
            bitmap.Save(filePath);
            Console.WriteLine($"Tag cloud visualization saved to file {filePath}");
        }
    }
}