using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class Visualizator : IVisualizator
    {
        private readonly Color penColor = Color.Blue;
        private readonly Size imageSize;
        private readonly ICollection<Rectangle> rectangles;

        public Visualizator(Size imageSize, ICollection<Rectangle> rectangles)
        {
            if (imageSize.Width <= 0 || imageSize.Height <= 0)
            {
                throw new ArgumentException(
                    "Width and height of image have to be > 0",
                    nameof(imageSize));
            }

            this.imageSize = imageSize;
            this.rectangles = rectangles ?? throw new ArgumentNullException(nameof(rectangles));
        }

        public Bitmap Generate()
        {
            var image = new Bitmap(imageSize.Width, imageSize.Height);
            var graphics = Graphics.FromImage(image);
            var pen = new Pen(penColor);
            var center = new Point(image.Width / 2, image.Height / 2);

            var rects = rectangles
                .Select(rectangle =>
                {
                    var location = new Point(
                        rectangle.Location.X + center.X,
                        rectangle.Location.Y + center.Y);

                    return new Rectangle(location, rectangle.Size);
                })
                .ToArray();

            if (rectangles.Count != 0)
            {
                graphics.DrawRectangles(pen, rects);
            }

            return image;
        }
    }
}