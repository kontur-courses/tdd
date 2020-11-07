using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsVisualizator
    {
        private List<Rectangle> Rectangles { get; }

        public TagsVisualizator(List<Rectangle> rectangles)
        {
            Rectangles = rectangles;
        }

        public Bitmap GetBitmap()
        {
            var imageSize = GetImageSize();
            var pen = new Pen(Color.MediumVioletRed, 4);

            var bitmap = new Bitmap(imageSize.Width + (int)pen.Width,
                imageSize.Height + (int)pen.Width);
            using var graphics = Graphics.FromImage(bitmap);

            if (Rectangles.Count != 0)
            {
                graphics.DrawRectangles(pen, Rectangles.ToArray());
            }

            return bitmap;
        }

        private Size GetImageSize()
        {
            var width = 0;
            var height = 0;

            foreach (var rectangle in Rectangles)
            {
                if (rectangle.Left < 0 || rectangle.Top < 0)
                {
                    throw new ArgumentException("rectangle out of image boundaries");
                }

                if (width < rectangle.Right)
                {
                    width = rectangle.Right;
                }
                if (height < rectangle.Bottom)
                {
                    height = rectangle.Bottom;
                }
            }

            return new Size(width, height);
        }
    }
}