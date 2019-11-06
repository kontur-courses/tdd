using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        private Spiral spiral;
        private readonly IEnumerator<Point> spiralEnumerator;
        private readonly List<Rectangle> rectangles;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            rectangles = new List<Rectangle>();
            spiral = new Spiral(2, 10);
            spiralEnumerator = spiral.GetNextPoint().GetEnumerator();
            spiralEnumerator.MoveNext();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            while (true)
            {
                var currentPoint = spiralEnumerator.Current;
                spiralEnumerator.MoveNext();
                var possibleRectangles = GeometryUtils.GetPossibleRectangles(currentPoint, rectangleSize);
                var acceptableRectangles = possibleRectangles
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
