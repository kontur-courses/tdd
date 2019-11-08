using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        private readonly Spiral spiral;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            spiral = new Spiral(1, 10);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var currentPoint = spiral.GetNextPoint();
                var possibleRectangles = GeometryUtils.GetPossibleRectangles(currentPoint, rectangleSize);
                var closerPossibleRectangles = possibleRectangles
                    .SelectMany(r => GeometryUtils.GetRectanglesThatCloserToPoint(Center, r, 1));
                var acceptableRectangles = possibleRectangles
                    .Concat(closerPossibleRectangles)
                    .Where(possibleRectangle =>
                        !rectangles.Any(r =>
                            GeometryUtils.RectanglesAreIntersected(r, possibleRectangle)))
                    .ToArray();
                if (!acceptableRectangles.Any())
                    continue;
                var rectangle = DistanceUtils.GetClosestToThePointRectangle(Center, acceptableRectangles);
                rectangles.Add(rectangle);
                return rectangle;
            }
        }
    }
}
