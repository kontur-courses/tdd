using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles, int size = 1500)
        {
            var bitmap = new Bitmap(size, size);
            var bitmapCenter = new Point(size / 2, size / 2);
            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White);
                foreach (var rectangle in rectangles)
                    g.DrawRectangle(
                        new Pen(Brushes.BlueViolet, 8),
                        rectangle.MoveRelativeToPoint(bitmapCenter));
            }

            return bitmap;
        }
    }
}