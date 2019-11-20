using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TagsCloudVisualization.Tests;

namespace TagsCloudVisualization
{
    public class Visualizer
    {
        private readonly List<Pen> pens = new List<Pen>
        {
            new Pen(Color.Black),
            new Pen(Color.Red),
            new Pen(Color.Blue),
            new Pen(Color.Green)
        };

        public Bitmap DrawRectangles(IEnumerable<Rectangle> rectangles)
        {
            var rectsAsArray = rectangles as Rectangle[] ?? rectangles.ToArray();
            var imageSize = GetSuitableImageSize(rectsAsArray);
            var center = new Point(imageSize.Width / 2, imageSize.Height / 2);
            var bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(bitmap);
            var random = new Random();
            graphics.FillRectangle(Brushes.White, 0, 0, imageSize.Width, imageSize.Height);
            foreach (var rectangle in rectsAsArray)
            {
                var shiftedRectangle = new Rectangle(new Point(center.X + rectangle.Location.X,
                    center.Y + rectangle.Location.Y), rectangle.Size);
                graphics.DrawRectangle(pens[random.Next(pens.Count)], shiftedRectangle);
            }
            return bitmap;
        }

        private Size GetSuitableImageSize(IEnumerable<Rectangle> rectangles)
        {
            var origin = new Point(0, 0);
            var imageSide = (int) rectangles
                .Select(x => Math.Max(x.Height, x.Width) + x.Location.GetDistanceTo(origin))
                .Max() * 2;
            return new Size(imageSide, imageSide);
        }
    }
}
