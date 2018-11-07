using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class RectanglesRenderer
    {
        public static Image GenerateImage(List<Rectangle> rectangles, float lineWidth = 6f)
        {
            var maxX = 0;
            var maxY = 0;
            var maxWidth = 0;
            var maxHeight = 0;

            foreach (var rectangle in rectangles)
            {
                maxX = Math.Max(maxX, rectangle.X);
                maxY = Math.Max(maxY, rectangle.Y);
                maxWidth = Math.Max(maxWidth, rectangle.Width);
                maxHeight = Math.Max(maxHeight, rectangle.Height);
            }

            var bmp = new Bitmap(maxX + maxWidth + 500, maxY + maxHeight + 500);
            var graphics = Graphics.FromImage(bmp);
            rectangles.ToList().ForEach(r => graphics.DrawRectangle(new Pen(Color.Black, 6f), r));
            return bmp;
        }
    }
}
