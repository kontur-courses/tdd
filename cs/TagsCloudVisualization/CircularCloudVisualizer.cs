using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualizer
    {
        public Bitmap DrawRectangles(IList<Rectangle> rectangles, int radius)
        {
            var bmp = new Bitmap(radius*2, radius*2);
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
