using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class Visualization
    {
        private readonly Random rng;
        private readonly Bitmap bitmap;

        public Visualization(int width, int height)
        {
            rng = new Random();
            bitmap = new Bitmap(width, height);
        }

        public void DrawRectangles(IEnumerable<Rectangle> rects)
        {
            var graphics = Graphics.FromImage(bitmap);
            foreach (var rect in rects)
            {
                Brush brush = new SolidBrush(Color.FromArgb(rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255)));
                graphics.FillRectangle(brush, rect);
            }
            bitmap.Save("img.bmp");
        }
    }
}