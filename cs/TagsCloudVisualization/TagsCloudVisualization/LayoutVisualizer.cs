using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class LayoutVisualizer
    {
        private readonly Bitmap canvas;
        private readonly Color color = Color.Black;
        private readonly Graphics graphics;
        private readonly int marginSize;
        private readonly Pen pen;

        public LayoutVisualizer(IReadOnlyCollection<Rectangle> layout, int marginSize = 50)
        {
            if (layout.Count == 0)
                throw new ArgumentException("Layout must not be empty");

            this.marginSize = marginSize;
            var coveringRectangle = GetCoveringRectangle(layout);
            var topLeftCorner = coveringRectangle.Location;
            canvas = new Bitmap(
                coveringRectangle.Width + 2 * this.marginSize,
                coveringRectangle.Height + 2 * this.marginSize);
            graphics = Graphics.FromImage(canvas);
            graphics.TranslateTransform(this.marginSize - topLeftCorner.X, this.marginSize - topLeftCorner.Y);
            pen = new Pen(color);
            graphics.DrawRectangles(pen, layout.ToArray());
        }

        private Rectangle GetCoveringRectangle(IEnumerable<Rectangle> rectangles)
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var rectangle in rectangles)
            {
                minX = Math.Min(minX, rectangle.Left);
                minY = Math.Min(minY, rectangle.Top);
                maxX = Math.Max(maxX, rectangle.Right);
                maxY = Math.Max(maxY, rectangle.Bottom);
            }

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public void SaveAs(string path)
        {
            canvas.Save(path);
        }
    }
}