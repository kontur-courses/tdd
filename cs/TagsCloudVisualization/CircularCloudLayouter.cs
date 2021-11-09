using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }

        private readonly List<Rectangle> rectangles = new();


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var location = GetRectangleLocation(rectangleSize);
            var nextRectangle = new Rectangle(location, rectangleSize);

            rectangles.Add(nextRectangle);

            return nextRectangle;
        }


        public Point GetRectangleLocation(Size rectangleSize)
        {
            if (rectangles.Count == 0)
                return GeometryHelper
                    .GetRectangleLocationFromCentre(Center, rectangleSize);

            return new Point(0, 0);
        }


        public IReadOnlyList<Rectangle> GetRectangles()
            => rectangles;
    }
}