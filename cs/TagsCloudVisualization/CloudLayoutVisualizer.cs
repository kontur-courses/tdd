using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudLayoutVisualizer
    {
        private static int imageBorders = 20;
        
        public static void SaveAsPngImage(IEnumerable<Rectangle> rects, string filePath)
        {
            var outerRect = Rectangle.GetOuterRect(rects);
            
            var bitmap = new Bitmap(
                outerRect.Size.Width + imageBorders * 2,
                outerRect.Size.Height + imageBorders * 2
            );
            
            var canvas = Graphics.FromImage(bitmap);
            var offsetX = outerRect.Pos.X * -1 + imageBorders;
            var offsetY = outerRect.Pos.Y * -1 + imageBorders;

            foreach (var rect in rects)
            {
                canvas.DrawRectangle(
                    new Pen(Color.Black, 1),
                    rect.Pos.X + offsetX,
                    rect.Pos.Y + offsetY,
                    rect.Size.Width,
                    rect.Size.Height
                );
            }

            bitmap.Save(filePath.ToString(), ImageFormat.Png);
        }
    }
}