using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        public Color BackColor { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }

        public Visualizer(Color fillColor, Color borderColor, Color backColor)
        {
            BackColor = backColor;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public Bitmap RenderBitmapFromRectangles(IEnumerable<Rectangle> rectangles)
        {
            var cloudRange = Geometry.GetMaxDistanceToRectangles(new Point(), rectangles);
            var renderSize = (int)((cloudRange / Math.Sqrt(2)) * 1.1);

            var bitmap = new Bitmap(renderSize, renderSize);
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, renderSize, renderSize);
            foreach (var r in rectangles)
            {
                graphics.FillRectangle(new SolidBrush(FillColor), r);
                graphics.DrawRectangle(new Pen(BorderColor), r);
            }

            return bitmap;
        }
    }
}
