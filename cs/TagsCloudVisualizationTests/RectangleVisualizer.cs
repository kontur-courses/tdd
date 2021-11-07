using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class RectangleVisualizer : IVisualizer
    {
        private readonly List<Rectangle> rectangles;
        private readonly Size size;

        public RectangleVisualizer(Size bitmapSize, IEnumerable<Rectangle> rectangles)
        {
            this.rectangles = rectangles.ToList();
            size = bitmapSize;
        }

        public void Draw(Graphics graphics)
        {
            var pen = new Pen(Color.OrangeRed, 1);
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(pen, rectangle);
        }

        public Size GetBitmapSize() => size;
    }
}