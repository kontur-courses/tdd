using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
            var graphics = Graphics.FromImage(bitmap);
            graphics.FillRectangle(new SolidBrush(BackColor), 0, 0, width, height);
            foreach (var r in rectangles)
            {
                graphics.FillRectangle(new SolidBrush(FillColor), r);
                graphics.DrawRectangle(new Pen(BorderColor), r);
            }

            bitmap.Save(fileName + ".bmp");
        }
    }
}
