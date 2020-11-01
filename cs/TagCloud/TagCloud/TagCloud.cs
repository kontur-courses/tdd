using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        private readonly Point center;
        public List<Rectangle> Rectangles { get; }
        private readonly IDiscreteCurve spiral;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Rectangles = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var currentPoint = spiral.CurrentPoint;
                var possibleRectangle = new Rectangle(currentPoint, rectangleSize);
                var canFit = Rectangles.All(rect => !rect.IntersectsWith(possibleRectangle));
                spiral.Next();
                if (!canFit) continue;
                Rectangles.Add(possibleRectangle);
                return possibleRectangle;
            }
        }
    }
}