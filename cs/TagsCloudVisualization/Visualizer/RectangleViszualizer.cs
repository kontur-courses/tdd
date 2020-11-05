using System;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public class RectangleVisualizer : IVisualizer
    {
        private readonly int dx = int.MaxValue;
        private readonly int dy = int.MaxValue;
        private readonly SolidBrush brush = new SolidBrush(Color.SlateGray);
        private readonly Rectangle[] rectangles;

        public RectangleVisualizer(Rectangle[] rectangles)
        {
            foreach (var rectangle in rectangles)
            {
                dx = Math.Min(dx, rectangle.X);
                dy = Math.Min(dy, rectangle.Y);
            }

            this.rectangles = rectangles;
        }

        public void Draw(Graphics graphics)
        {
            graphics.TranslateTransform(-dx, -dy);
            graphics.FillRectangles(brush, rectangles);
        }
    }
}
