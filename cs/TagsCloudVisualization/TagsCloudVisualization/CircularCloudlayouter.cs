using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; set; }
        private readonly FirstSector _firstSector;
        private readonly Direction _direction;
        public List<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("both center coordinates should be non-negative");

            Center = center;
            _firstSector = new FirstSector();
            _direction = new Direction();
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {   
            // В текущий момент подразумеваем 0 < direction < Math.PI / 2
            var result = _firstSector.PlaceNewRectangle(_direction.GetNextDirection(), rectangleSize);
            Rectangles.Add(result);

            return result;
        }
    }
}
