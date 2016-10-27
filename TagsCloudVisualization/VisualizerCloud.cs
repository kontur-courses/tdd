using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class VisualizerCloud : IVisualizerCloud
    {
        private readonly Bitmap bitmap;
        private readonly Graphics graphics;
        private readonly SolidBrush brush;
        private readonly StringFormat drawFormat;


        public VisualizerCloud(int width, int height, SolidBrush brush)
        {
            this.brush = brush;
            bitmap = new Bitmap(width, height);
            graphics = Graphics.FromImage(bitmap);
            drawFormat = new StringFormat();
        }

        private void DrawPhrase(string phrase, Rectangle frameRectangle, Font font)
        {
            graphics.DrawString(
                phrase,
                font,
                brush,
                frameRectangle,
                drawFormat
            );
        }

        private void DrawRectangle(Rectangle rectangle, Color color)
        {
            graphics.DrawRectangle(
                new Pen(color, 3), rectangle
            );
        }


        public Bitmap GetImageCloud(List<Tuple<Rectangle, string, Font>> blocks, Color backgroundСolor)
        {
            graphics.Clear(backgroundСolor);
            foreach (var block in blocks)
            {
                DrawPhrase(block.Item2, block.Item1, block.Item3);
            }
            return bitmap;
        }

        public Bitmap GetImageCloud(List<Rectangle> rectangles, Color colorOfRectangle, Color backgroundСolor)
        {
            graphics.Clear(backgroundСolor);
            foreach (var rectangle in rectangles)
            {
                DrawRectangle(rectangle, colorOfRectangle);
            }
            return bitmap;
        }
    }
}