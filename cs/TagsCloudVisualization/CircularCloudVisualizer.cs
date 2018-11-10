using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        public Bitmap DrawCloud(IList<Rectangle> rectangles)
        {
            var radius = rectangles.GetSurroundingCircleRadius();
            var bmp = new Bitmap(radius * 2, radius * 2);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DimGray);
                if (rectangles.Any())
                    foreach (var rectangle in rectangles)
                        g.DrawRectangle(new Pen(Brushes.Salmon, 8), rectangle.ShiftRectangleToBitMapCenter(bmp));
            }
            return bmp;
        }
    }
}
