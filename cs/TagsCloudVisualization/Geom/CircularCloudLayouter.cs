using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Geom
{
    public class CircularCloudLayouter
    {
        private List<Rectangle> rectangles;
        private Spiral spiral;

        public IReadOnlyCollection<Rectangle> Rectangles => new ReadOnlyCollection<Rectangle>(rectangles);
        public Point Center => spiral.Center;
        public readonly Size CloudSize;

        public CircularCloudLayouter(int locationX, int locationY, int width, int height)
            : this(new Point(locationX, locationY), new Size(width, height))
        {

        }

        public CircularCloudLayouter(Point center, Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException(nameof(size));

            spiral = new Spiral(center);
            rectangles = new List<Rectangle>();
            CloudSize = size;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height > CloudSize.Height || rectangleSize.Width > CloudSize.Width)
                throw new ArgumentException("Should be less then CloudSize", nameof(rectangleSize));

            while (true)
            {
                var location = spiral.GetNextLocation();
                var rectangle = new Rectangle((int)location.X, (int) location.Y, rectangleSize.Width, rectangleSize.Height);

                if (!rectangles.Any(rectangle.IntersectsWith))
                {
                    rectangles.Add(rectangle);
                    return rectangle;
                }
            }
        }
    }
}