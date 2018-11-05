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
        public Bitmap DrawRectangles(CircularCloudLayouter layouter)
        {
            var bmp = new Bitmap(layouter.Radius*2, layouter.Radius*2);
            using (var g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.DimGray);
                if (layouter.Rectangles.Any())
                    layouter
                        .Rectangles
                        .ForEach(rect => g.DrawRectangle(new Pen(Brushes.Salmon, 8), rect.ShiftRectangleToBitMapCenter(bmp)));
            }
            return bmp;
        }
    }
}
