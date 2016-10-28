using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CloudRectangleVisualizer : IRectangleCloudVisualizer
    {
        private int width;
        private int height;
        public int Width
        {
            get { return width; }
            set
            {
                if (width < 0)
                    throw new ArgumentException("width value must be a positive number");
                width = value;
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (height < 0)
                    throw new ArgumentException("height value must be a positive number");
                height = value;
            }
        }
        public CloudRectangleVisualizer(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Bitmap GetImageCloud(List<Rectangle> rectangles, Color colorOfRectangle, Color backgroundСolor)
        {
            var bitmap = new Bitmap(Width, Height);
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

        private void DrawRectangle(Graphics graphics, Rectangle rectangle, Color color)
        {
            graphics.DrawRectangle(
                new Pen(color, 3), rectangle
            );
        }
    }
}