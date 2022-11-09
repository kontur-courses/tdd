using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudVisualization
    {
        private Bitmap canvas;
        private Graphics graphics;

        public CircularCloudVisualization(Size canvasSize)
        {
            canvas = new Bitmap(canvasSize.Width, canvasSize.Height);
            graphics = Graphics.FromImage(canvas);
            graphics.Clear(Color.White);
        }

        public void DrawRectangles(List<Rectangle> rectangles)
        {
            foreach (var rectangle in rectangles)
                graphics.DrawRectangle(Pens.Black, rectangle);
        }

        public void SaveCanvas(String path)
        {
            canvas.Save(path);
        }
    }
}