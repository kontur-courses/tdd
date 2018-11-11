using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private readonly IEnumerator<Point> spiralGenerator;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            spiralGenerator = new RoundSpiralGenerator(center, 0.1).GetEnumerator();
            spiralGenerator.MoveNext();
        }

        public Point Center { get; }

        public IEnumerable<Rectangle> Rectangles => rectangles;

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("size has non positive parts");

            var nextPosition = spiralGenerator.Current;
            var rectangle = new Rectangle(nextPosition, rectangleSize);
            while (DoesIntersectWithPreviousRectangles(rectangle))
            {
                spiralGenerator.MoveNext();
                nextPosition = spiralGenerator.Current;
                rectangle = new Rectangle(nextPosition, rectangleSize);
            }

            rectangle = AdjustPosition(rectangle);
            rectangles.Add(rectangle);
            return rectangle;
        }

        private Rectangle AdjustPosition(Rectangle rectangle)
        {
            var centerDirection = Center - rectangle.Location;
            centerDirection *= 0.2;
            rectangle.Location.Offset(centerDirection);
            centerDirection *= -1 / centerDirection.Length;
            while (DoesIntersectWithPreviousRectangles(rectangle))
                rectangle.Location.Offset(centerDirection);

            return rectangle;
        }

        private bool DoesIntersectWithPreviousRectangles(Rectangle rectangle) =>
            rectangles.Any(rectangle.IntersectsWith);

        public IEnumerable<Rectangle> PutNextRectangles(IEnumerable<Size> rectanglesSizes) =>
            rectanglesSizes.Select(PutNextRectangle);
    }
}
