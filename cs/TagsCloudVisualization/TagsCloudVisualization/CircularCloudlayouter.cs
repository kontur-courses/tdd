using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; set; }
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

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var result = _rectangleStorage.PlaceNewRectangle(rectangleSize);
            Rectangles.Add(result);

            return result;
        }

        public static bool IsRectanglesIntersect(Rectangle first, Rectangle second)
        {
            if (first.X > second.X)
            {
                var tmp = new Rectangle(first.X, first.Y, first.Width, first.Height);
                first = second;
                second = tmp;
            }

            var xIntersects = first.X + first.Width > second.X;
            var yIntersects = second.Y > first.Y - first.Height && second.Y - second.Height < first.Y;

            return xIntersects && yIntersects;
        }

        public static bool IsRectanglesIntersect(List<Rectangle> rectangles)
        {
            for(var i = 0; i < rectangles.Count; i++)
                if (rectangles.Where((t, j) => i != j && IsRectanglesIntersect(rectangles[i], t)).Any())
                    return true;

            return false;
        }
    }
}
