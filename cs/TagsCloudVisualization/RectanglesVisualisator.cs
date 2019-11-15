using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class RectanglesVisualisator : IDisposable
    {
        private List<Rectangle> rectanglesList;
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

        public static void SaveNewRectangles(List<Rectangle> newRectangles, string filename)
        {
            using (var visualisator = new RectanglesVisualisator(new Size(2000, 2000), newRectangles))
            {
                visualisator.DrawRectangles();
                visualisator.Save(filename);
            }
        }

        public void Dispose()
        {
            bitmap?.Dispose();
            graphics?.Dispose();
        }
    }
}
