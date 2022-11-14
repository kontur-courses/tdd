using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class LayoutVisualizer
    {
        private Bitmap canvas;
        public Color Color = Color.Black;
        public int MarginSize;
        public LayoutVisualizer(List<Rectangle> layout, Size canvasSize, Point topLeftCorner, int marginSize = 50)
        {
            MarginSize = marginSize;
            canvas = new Bitmap(canvasSize.Width + 2 * MarginSize, canvasSize.Height + 2 * MarginSize);
            var graphics = Graphics.FromImage(canvas);
            var pen = new Pen(Color);
            graphics.TranslateTransform(topLeftCorner.X + MarginSize, topLeftCorner.Y + MarginSize);
            graphics.DrawRectangles(pen, layout.ToArray());
        }

        public static LayoutVisualizer FromCircularCloudLayouter(CircularCloudLayouter layouter)
        {
            var coveringCircleRadius = (int) Math.Ceiling(layouter.GetCoveringCircleRadius());
            var sideLength = coveringCircleRadius * 2;
            return new LayoutVisualizer(layouter.Rectangles.ToList(), 
                new Size(sideLength, sideLength),
                new Point(coveringCircleRadius, coveringCircleRadius));
        }

        public void SaveAs(string path)
        {
            canvas.Save(path);
        }
    }
}