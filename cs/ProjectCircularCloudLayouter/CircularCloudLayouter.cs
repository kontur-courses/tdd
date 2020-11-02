using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectCircularCloudLayouter
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        public readonly ISpiral Spiral;
        public int CloudRadius { get; private set; }

        public CircularCloudLayouter(Point center)
        {
            _rectangles = new List<Rectangle>();
            Spiral = new ArchimedeanSpiral(center);
        }

        public Rectangle GetCurrentRectangle => _rectangles.Last();

        public IReadOnlyList<Rectangle> GetRectangles => _rectangles;

        public void PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Size width and height must be positive");
            while (true)
            {
                var currentSpiralPosition = Spiral.GetNewSpiralPoint();
                var rectangle = new Rectangle(currentSpiralPosition, rectangleSize);
                if (IsAnyIntersectWithRectangles(rectangle, _rectangles)) continue;
                _rectangles.Add(rectangle);
                // if (_rectangles.Count>5)
                //     _spiralAngle -= 360 * 0.017; - в таком случае прямоуголники будут раставляться немного плотнее
                // если же применить команду _spiralAngle = 0; то добьемся максимальной плотности, но скажется скорости

                UpdateCloudRadius(rectangle);
                break;
            }
        }

        public static bool IsAnyIntersectWithRectangles(Rectangle rectangleToCheck, List<Rectangle> rectangles) =>
            rectangles.Any(rec => rec.IntersectsWith(rectangleToCheck));

        public static int GetCeilingDistanceBetweenPoints(Point first, Point second) =>
            (int) Math.Ceiling(Math.Sqrt((first.X - second.X) * (first.X - second.X) +
                                         (first.Y - second.Y) * (first.Y - second.Y)));

        private void UpdateCloudRadius(Rectangle currentRectangle)
        {
            var maxDistance = new[]
            {
                GetCeilingDistanceBetweenPoints(currentRectangle.Location, Spiral.Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + new Size(currentRectangle.Width, 0),
                    Spiral.Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + new Size(0, currentRectangle.Height),
                    Spiral.Center),
                GetCeilingDistanceBetweenPoints(currentRectangle.Location + currentRectangle.Size, Spiral.Center)
            }.Max();
            if (maxDistance > CloudRadius)
                CloudRadius = maxDistance;
        }
    }
}