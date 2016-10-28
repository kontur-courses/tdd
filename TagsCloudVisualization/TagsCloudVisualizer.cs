using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class TagsCloudVisualizer : ITagsCloudVisualizer
    {
        public SolidBrush Brush { get; set; }
        private readonly StringFormat drawFormat;
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




        public TagsCloudVisualizer(int width, int height, SolidBrush brush)
        {
            Brush = brush;
            Width = width;
            Height = height;
            drawFormat = new StringFormat();
        }

        public Bitmap GetImageCloud(List<Tag> tags, Color backgroundСolor)
        {
            var bitmap = new Bitmap(Width, Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(backgroundСolor);
                foreach (var tag in tags)
                {
                    DrawPhrase(graphics, tag.Phrase, tag.Rectangle, tag.Font);
                }
            }
            return bitmap;
        }

        private void DrawPhrase(Graphics graphics, string phrase, Rectangle frameRectangle, Font font)
        {
            graphics.DrawString(
                phrase,
                font,
                Brush,
                frameRectangle,
                drawFormat
            );
        }

    }
}