using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CloudLayouterVisualizer
{
    public class Visualizer: IDisposable
    {
        private readonly Bitmap image;
        private readonly Pen pen;
        private readonly Graphics graphics;
        
        public Visualizer(Size imageSize, Pen pen)
        {
            this.pen = pen;
            image = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(image);
        }

        public void DrawRectangle(Rectangle rectangle)
        {
            graphics.DrawRectangle(pen, rectangle);
        }

        public void DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            graphics.DrawRectangles(pen, rectangles.ToArray());
        }

        public void SaveImage(string fileName)
        {
            image.Save(fileName);
        }

        public void Dispose()
        {
            graphics.Dispose();
            image.Dispose();
        }
    }
}