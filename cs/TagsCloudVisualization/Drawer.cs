using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Drawer
    {
        private readonly Bitmap canvas;
        private readonly Graphics graphics;

        public Drawer(Size canvasSize)
        {
            canvas = new Bitmap(canvasSize.Width, canvasSize.Height);
            graphics = Graphics.FromImage(canvas);
            graphics.Clear(Color.White);
        }

        public void DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(Pens.Black, rectangle);
        }

        public void SaveCanvas(string path)
        {
            canvas.Save(path);
        }
    }
}