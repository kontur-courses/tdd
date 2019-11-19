using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        public const int ImageWidth = 1920;
        public const int ImageHeight = 1080;
        private readonly List<Pen> pens = new List<Pen>
        {
            new Pen(Color.Black),
            new Pen(Color.Red),
            new Pen(Color.Blue),
            new Pen(Color.Green)
        };
        private readonly Point center;

        public Visualizer()
        {
            center = new Point(ImageWidth/2, ImageHeight/2);
        }

        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            var bitmap = new Bitmap(ImageWidth, ImageHeight);
            var graphics = Graphics.FromImage(bitmap);
            var random = new Random();
            graphics.FillRectangle(Brushes.White, 0, 0, ImageWidth, ImageHeight);
            foreach (var rectangle in rectangles)
            {
                var shiftedRectangle = new Rectangle(new Point(center.X + rectangle.Location.X,
                    center.Y + rectangle.Location.Y), rectangle.Size);
                graphics.DrawRectangle(pens[random.Next(pens.Count)], shiftedRectangle);
            }
            return bitmap;
        }
    }
}
