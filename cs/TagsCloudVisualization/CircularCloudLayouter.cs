using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }


        private readonly List<Rectangle> rectangles = new();
        private readonly HashSet<Point> usedPoints = new();
        private readonly CloudLayoutParameters layoutParameters = new();


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
            {
                throw new ArgumentException();
            }

            var nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            usedPoints.UnionWith(GeometryHelper.GetAllPointIntoRectangle(nextRectangle));

            return nextRectangle;
        }

        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count != 0)
                return SearchRectangleOnCircle(rectangleSize);

            var rectangleLocation = GeometryHelper.GetRectangleLocationFromCenter(Center, rectangleSize);

            return new Rectangle(rectangleLocation, rectangleSize);
        }

        private Rectangle SearchRectangleOnCircle(Size rectangleSize)
        {
            while (true)
            {
                if (!layoutParameters.IsValidNextAngle())
                {
                    layoutParameters.BoostRadius();
                }

                if (HasIntersectRectangles(rectangleSize, out var rectangle))
                {
                    continue;
                }

                layoutParameters.ResetRadius();

                return rectangle;
            }
        }

        private bool HasIntersectRectangles(Size rectangleSize, out Rectangle rectangle)
        {
            var rectangleCenter =
                GeometryHelper.GetPointOnCircle(Center, layoutParameters.Radius, layoutParameters.Angle);

            var rectangleLocation =
                GeometryHelper.GetRectangleLocationFromCenter(rectangleCenter, rectangleSize);

            rectangle = new Rectangle(rectangleLocation, rectangleSize);

            var rect = rectangle;

            return usedPoints.Contains(rectangleLocation)
                   || rectangles.Any(r => r.IntersectsWith(rect));
        }

        public IReadOnlyList<Rectangle> GetRectangles()
            => rectangles;
    }
}