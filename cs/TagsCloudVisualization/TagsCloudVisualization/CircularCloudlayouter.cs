using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly RectangleStorage _rectangleStorage;
        public List<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("both center coordinates should be non-negative");

            Center = center;
            _rectangleStorage = new RectangleStorage(Center, new Direction());
            Rectangles = new List<Rectangle>();
        }

        public Point Center { get; set; }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var result = _rectangleStorage.PlaceNewRectangle(rectangleSize);
            Rectangles.Add(result);

            return result;
        }
    }
}