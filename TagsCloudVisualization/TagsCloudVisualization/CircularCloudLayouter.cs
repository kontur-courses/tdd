using System;
using System.Linq;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : CloudLayouter
    {
        public readonly Point Center;
        private readonly Spiral _spiral;

        public CircularCloudLayouter(Point center, Spiral spiral) : base()
        {
            Center = center;
            _spiral = spiral;
        }

        public override Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"Ширина и высота прямоугольника должны быть положительными числами: " +
                    $"{rectangleSize.Width} x {rectangleSize.Height}");

            var rectangle = _rectangles.Any()
                ? GetNextRectangleWithLocation(rectangleSize)
                : new Rectangle(Center.GetRectangleLocationByCenter(rectangleSize), rectangleSize);
            _rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangleWithLocation(Size rectSize)
        {
            var dryRect = new Rectangle(Point.Empty, rectSize);
            var pointEnumerator = _spiral.GetDiscretePoints().GetEnumerator();
            while (_rectangles.Any(r => r.IntersectsWith(dryRect)))
            {
                pointEnumerator.MoveNext();
                dryRect.Location = pointEnumerator.Current;
            }
            return dryRect;
        }
    }
}
