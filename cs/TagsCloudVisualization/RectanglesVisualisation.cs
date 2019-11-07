using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TagsCloudVisualization
{
    class RectanglesVisualisator
    {
        private readonly List<Rectangle> rectangles;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public RectanglesVisualisator(Size imageSize, List<Rectangle> rectangles)
        {
            this.rectangles = rectangles;
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawRectangles()
        {
            var brush = Brushes.Green;
            graphics.FillRectangles(brush, rectangles.ToArray());
            var pen = new Pen(Color.Black);
            graphics.DrawRectangles(pen, rectangles.ToArray());
        }

        public void Save(string filename)
        {
            bitmap.Save(Environment.CurrentDirectory + @"\Images\" + filename);
        }
    }
}
