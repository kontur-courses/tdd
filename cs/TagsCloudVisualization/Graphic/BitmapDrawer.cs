using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Infrastructure.Environment;

namespace TagsCloudVisualization.Graphic
{
    public class BitmapDrawer : IEnvironmentToImageConverter<Rectangle>
    {
        private readonly int fieldSize;
        private readonly Random random;
        public BitmapDrawer()
        {
            fieldSize = 10;
            random = new Random();
        }
        public Image GetEnvironmentImage(Environment<Rectangle> environment)
        {
            var (left, right, bottom, top) = GetBoarders(environment);
            var image = new Bitmap(right - left + fieldSize, bottom - top + fieldSize);
            var imageGraphics = Graphics.FromImage(image);
            var rectangles = environment.Select(rectangle =>
            {
                rectangle.Offset(-left + fieldSize / 2, -top + fieldSize / 2);
                return rectangle;
            });
            
            var pen = new Pen(Color.Red, 1);
            foreach (var rectangle in rectangles)
            {
                pen.Color = GetRandomColor();
                imageGraphics.DrawRectangle(pen, rectangle);
            }
            return image;
        }
        
        private Color GetRandomColor()
        {
            return Color.FromArgb(128,
                255 * random.Next(2),
                255 * random.Next(2),
                255 * random.Next(2));
        }

        private (int left, int right, int bottom, int top) GetBoarders(IEnumerable<Rectangle> environment)
        {
            var left = 0;
            var right = 0;
            var bottom = 0;
            var top = 0;
            foreach (var rectangle in environment)
            {
                if (rectangle.Left < left)
                    left = rectangle.Left;
                if (rectangle.Right > right)
                    right = rectangle.Right;
                if (rectangle.Top < top)
                    top = rectangle.Top;
                if (rectangle.Bottom > bottom)
                    bottom = rectangle.Bottom;
            }

            return (left, right, bottom, top);
        }
    }
}