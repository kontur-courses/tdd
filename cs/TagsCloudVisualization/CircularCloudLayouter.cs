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
        public readonly CloudLayouterParameters Parameters = new();


        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }


        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height < 0 || rectangleSize.Width < 0)
                throw new ArgumentException();

            var nextRectangle = GetNextRectangle(rectangleSize);

            rectangles.Add(nextRectangle);
            usedPoints.UnionWith(GeometryHelper.GetAllPointIntoRectangle(nextRectangle));

            return nextRectangle;
        }


        private Rectangle GetNextRectangle(Size rectangleSize)
        {
            if (rectangles.Count != 0)
                return SearchRectangleOnCircle(rectangleSize);

            var rectangleLocation = GeometryHelper
                .GetRectangleLocationFromCenter(Center, rectangleSize);
            
            return new Rectangle(rectangleLocation, rectangleSize);
        }

        private Rectangle SearchRectangleOnCircle(Size rectangleSize)
        {
            while (true)
            {
                while (Parameters.IsNextAngleLessThanPi2())
                {
                    var rectangleCenter =
                        GeometryHelper.GetPointOnCircle(Center, Parameters.CurrentRadius, Parameters.NextAngle);

                    var rectangleLocation =
                        GeometryHelper.GetRectangleLocationFromCenter(rectangleCenter, rectangleSize);

                    if (usedPoints.Contains(rectangleLocation)) continue;

                    var rectangle = new Rectangle(rectangleLocation, rectangleSize);

                    if (rectangles.Any(r => r.IntersectsWith(rectangle))) continue;

                    Parameters.ResetRadius();
                    return rectangle;
                }

                Parameters.BoostRadius();
            }
        }


        public IReadOnlyList<Rectangle> GetRectangles()
            => rectangles;
    }
}