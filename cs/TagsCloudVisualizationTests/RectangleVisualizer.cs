using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualizationTests
{
    public class RectangleVisualizer : Visualizer
    {
        private readonly List<Rectangle> rectangles;

        public RectangleVisualizer(Size bitmapSize, IEnumerable<Rectangle> rectangles) : base(bitmapSize)
        {
            this.rectangles = rectangles.ToList();
        }

        protected override void Draw(Graphics graphics)
        {
            var pen = new Pen(Color.OrangeRed, 1);
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(pen, rectangle);
        }
    }
}