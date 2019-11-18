using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class Visualizer
    {
        public Bitmap Visualize(List<Rectangle> rectangles, Size imageSize)
        {
            var bmp = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bmp);
            var rnd = new Random();
            foreach (var rectangle in rectangles)
                graphics.FillRectangle(new SolidBrush(GetRandomColor(rnd)), rectangle);
            return bmp;
        }

        private Color GetRandomColor(Random rnd)
        {
            return Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        }
    }
}
