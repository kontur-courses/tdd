using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        private readonly Pen pen = new Pen(Brushes.BlueViolet, 1);

        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, int radius = 500)
        {
            var bitmap = new Bitmap(radius * 2, radius * 2);
            var bitmapCenter = new Point(radius, radius);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                foreach (var rectangle in rectangles)
                    g.DrawRectangle(pen, rectangle.MoveRelativeToPoint(bitmapCenter));
            }

            return bitmap;
        }
    }
}