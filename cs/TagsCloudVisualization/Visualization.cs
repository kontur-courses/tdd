using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class Visualization
    {
        public static Bitmap VisualizationFil(HashSet<Rectangle> rectangles, int width, int height)
        {
            if(width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width));
            
            if(height <= 0)
                throw  new ArgumentOutOfRangeException(nameof(height));
            
            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);
            var rnd = new Random();
            
            foreach (var rectangle in rectangles)
            {
                var color = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                graphics.FillRectangle(new SolidBrush(color), rectangle);
            }

            return bitmap;
        }
        
    }
}