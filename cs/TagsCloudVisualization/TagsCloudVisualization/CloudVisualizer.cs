using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TagsCloudVisualization
{
    public static class CloudVisualizer
    {
        public static Bitmap Draw(CircularCloudLayouter layouter, int numberOfTags, List<Color> colors, Size size)
        {
            var bitmap = new Bitmap(layouter.Size.Width, layouter.Size.Height);
            var graphics = Graphics.FromImage(bitmap);
            var rnd = new Random();

            for (var i = 0; i < numberOfTags; i++)
            {
                var pen = new Pen(colors[rnd.Next(colors.Count)], 0) {Alignment = PenAlignment.Inset};
                graphics.DrawRectangle(pen, layouter.PutNextRectangle(size));
            }

            return bitmap;
        }
    }
}