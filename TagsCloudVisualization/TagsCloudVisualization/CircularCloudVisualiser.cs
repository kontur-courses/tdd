using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    class CircularCloudVisualiser
    {
        public Color BackColor { get; set; }
        public Color FillColor { get; set; }
        public Color BorderColor { get; set; }

        public CircularCloudVisualiser(Color fillColor, Color borderColor, Color backColor)
        {
            BackColor = backColor;
            FillColor = fillColor;
            BorderColor = borderColor;
        }

        public void SaveBitmap(string fileName, int width, int height, IEnumerable<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(width, height);
            var g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(BackColor), 0, 0, width, height);
            foreach (var r in rectangles)
            {
                g.FillRectangle(new SolidBrush(FillColor), r);
                g.DrawRectangle(new Pen(BorderColor), r);
            }

            bitmap.Save(fileName + ".bmp");
        }
    }
}
