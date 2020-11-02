using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private Point center;
        public List<Rectangle> Rectangles { get; }
        private SpiralProvider spiral;
        private int maxX = -1;
        private int maxY = -1;

        public CircularCloudLayouter(Point point)
        {
            if (point.X < 0 || point.Y < 0)
                throw new ArgumentException();

            center = point;
            Rectangles = new List<Rectangle>();
            spiral = new SpiralProvider(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException();

            var rectangle = spiral.GetRectangle(rectangleSize, Rectangles);
            Rectangles.Add(rectangle);

            if (rectangle.Location.X > maxX)
                maxX = rectangle.Location.X;

            if (rectangle.Location.Y > maxY)
                maxY = rectangle.Location.Y;

            return rectangle;
        }

        public void CreateImage(string fileName)
        {
            Drawer.DrawImage(Rectangles, center, maxX, maxY, fileName);
        }
    }
}

