using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class RectanglesVisualisator
    {
        private readonly List<Rectangle> rectanglesList;
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;

        public RectanglesVisualisator(Size imageSize, List<Rectangle> rectangles)
        {
            rectanglesList = rectangles;
            bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            graphics = Graphics.FromImage(bitmap);
        }

        public void DrawRectangles()
        {
            graphics.FillRectangles(Brushes.Blue, rectanglesList.ToArray());
            var pen = new Pen(Color.FromArgb(255, 0, 255, 0), 2);
            graphics.DrawRectangles(pen, rectanglesList.ToArray());
        }

        public void Save(string filename)
        {
            bitmap.Save(Environment.CurrentDirectory + @"\RectangleImages\" + filename);
        }
    }
}
