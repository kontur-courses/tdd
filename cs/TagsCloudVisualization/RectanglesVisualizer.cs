using System;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RectanglesVisualizer
    {
        private readonly Rectangle[] rectangles;

        public RectanglesVisualizer(Rectangle[] rectangles)
        {
            this.rectangles = rectangles;
        }

        public Bitmap RenderToBitmap()
        {
            var size = CalculateBitmapSize();
            var bitmap = new Bitmap(size.Width, size.Height);
            var graphics = Graphics.FromImage(bitmap);
            var pen = new Pen(Color.Black);

            graphics.Clear(Color.White);
            if (rectangles.Any())
                graphics.DrawRectangles(pen, rectangles);
            return bitmap;
        }

        private Size CalculateBitmapSize()
        {
            if (rectangles.Any())
            {
                var width = rectangles.Max(r => r.Right) + rectangles.Min(r => r.Left);
                var height = rectangles.Max(r => r.Bottom) + rectangles.Min(r => r.Top);
                return new Size(width, height);
            }
            return new Size(100, 100);
        }
    }
}
