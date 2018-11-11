using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CloudLayoutVisualizer
    {
        private const int ImageBorders = 20;
        private const int MaxWidth = 1024;
        private const int MaxHeight = 1024;
        
        public static void SaveAsPngImage(IEnumerable<Rectangle> rects, string filePath)
        {
            var outerRect = Rectangle.GetOuterRect(rects);

            var imgWidth = outerRect.Size.Width + ImageBorders * 2;
            var imgHeight = outerRect.Size.Height + ImageBorders * 2;
            
            var ratioX = (float)MaxWidth / imgWidth;
            var ratioY = (float)MaxHeight / imgHeight;
            var ratio = Math.Min(ratioX, ratioY);
            
            var bitmap = new Bitmap((int)(imgWidth * ratio), (int)(imgHeight * ratio));
            var graphics = Graphics.FromImage(bitmap);
            graphics.ScaleTransform(ratio, ratio);
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            
            var offsetX = outerRect.Pos.X * -1 + ImageBorders;
            var offsetY = outerRect.Pos.Y * -1 + ImageBorders;

            foreach (var rect in rects)
            {
                graphics.FillRectangle(
                    new SolidBrush(Color.Bisque), 
                    rect.Pos.X + offsetX,
                    rect.Pos.Y + offsetY,
                    rect.Size.Width,
                    rect.Size.Height
                );
                
                graphics.DrawRectangle(
                    new Pen(Color.Black, 2),
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