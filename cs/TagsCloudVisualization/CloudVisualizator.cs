using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace TagsCloudVisualization
{
    public class CloudVisualizator
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graph;
        private readonly Pen pen;
        private readonly Brush brush;

        public CloudVisualizator(int width, int height, Color borderColor, Color fillColor)
        {
            bitmap = new Bitmap(width, height);
            graph = Graphics.FromImage(bitmap);
            pen = new Pen(new SolidBrush(borderColor));
            brush = new SolidBrush(fillColor);
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            graph.FillRectangle(brush, rectangle);
            graph.DrawRectangle(pen, rectangle);
        }

        public void SaveImage(string filename, ImageFormat format)
        {
            bitmap.Save(filename, format);
        }
    }
}
