using System;
using System.Linq;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter : CloudLayouter
    {
        public readonly Point Center;
        private Spiral _spiral;

        public CircularCloudLayouter(Point center, Spiral spiral) : base()
        {
            Center = center;
            _spiral = spiral;
            _spiral.SetCenter(center);
        }

        public override Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width < 0 || rectangleSize.Height < 0)
                throw new ArgumentException($"Отрицательный размер прямоугольника: " +
                    $"{rectangleSize.Width} x {rectangleSize.Height}");

            var rectangle = Rectangles.Any()
                ? GetNextRectangleWithLocation(rectangleSize)
                : new Rectangle(Center.GetRectangleLocationByCenter(rectangleSize), rectangleSize);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle GetNextRectangleWithLocation(Size rectSize)
        {
            var dryRect = new Rectangle(Point.Empty, rectSize);
            var pointEnumerator = _spiral.GetDiscretePoints().GetEnumerator();
            while (pointEnumerator.MoveNext())
            {
                dryRect.Location = pointEnumerator.Current;
                if (Rectangles.Any(r => r.IntersectsWith(dryRect)))
                    break;
            }
            return dryRect;
        }
    }
}
