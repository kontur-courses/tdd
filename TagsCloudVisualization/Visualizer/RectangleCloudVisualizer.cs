using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization.Visualizer
{
    public class RectangleCloudVisualizer : IRectangleCloudVisualizer
    {

        public Bitmap GetImageCloud(List<Rectangle> rectangles, int width, int height, Color colorOfRectangle, Color backgroundСolor)
        {
            if (height < 0)
                throw new ArgumentException("height value must be a positive number");
            if (width < 0)
                throw new ArgumentException("width value must be a positive number");
            var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(backgroundСolor);
                foreach (var rectangle in rectangles)
                {
                    DrawRectangle(graphics, rectangle, colorOfRectangle);
                }
            }
            return bitmap;
        }

        private static void DrawRectangle(Graphics graphics, Rectangle rectangle, Color color)
        {
            graphics.DrawRectangle(
                new Pen(color, 3), rectangle
            );
        }
    }
}