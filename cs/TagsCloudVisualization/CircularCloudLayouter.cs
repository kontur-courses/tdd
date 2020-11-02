using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> Rectangles = new List<Rectangle>();
        private readonly List<Point> potentialPosingPoints;
        private Point Center { get; }
        private bool isFirst = true;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            potentialPosingPoints = new List<Point> {center};
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (isFirst)
                return PutFirstRectangle(rectangleSize);

            var rect = ChangeRectanglePosition(new Rectangle(Center, rectangleSize));
            Rectangles.Add(rect);
            potentialPosingPoints.AddRange(GetPotentialPosingPointsFromRectangle(rect));
            return rect;
        }

        public Point GetCenterPoint()
        {
            return Center;
        }

        private Rectangle PutFirstRectangle(Size rectangleSize)
        {
            var rect = new Rectangle(
                new Point(Center.X - rectangleSize.Width / 2, (Center.Y - rectangleSize.Height / 2)),
                rectangleSize);
            Rectangles.Add(rect);
            potentialPosingPoints.AddRange(GetPotentialPosingPointsFromRectangle(rect));
            isFirst = false;
            return rect;

        }

        private Rectangle ChangeRectanglePosition(Rectangle rect)
        {
            var orderedPoints = potentialPosingPoints.OrderBy(GetDistanceToCenter);
            foreach (var point in orderedPoints)
            {
                foreach (var location in GetLocationVariations(point, rect))
                {
                    rect.Location = location;
                    if (Rectangles.All(x => !x.IntersectsWith(rect)))
                        return rect;
                }
            }
            
            throw new ArithmeticException("Unknown error");
        }

        private double GetDistanceToCenter(Point firstPoint)
        {
            var dx = Math.Max(firstPoint.X, Center.X) - Math.Min(firstPoint.X, Center.X);
            var dy = Math.Max(firstPoint.Y, Center.Y) - Math.Min(firstPoint.Y, Center.Y);
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private List<Point> GetLocationVariations(Point potentialPoint, Rectangle rect)
        {
            return new List<Point>
            {
                new Point(potentialPoint.X, potentialPoint.Y),
                new Point(potentialPoint.X - rect.Width , potentialPoint.Y),
                new Point(potentialPoint.X - rect.Width, potentialPoint.Y - rect.Height),
                new Point(potentialPoint.X, potentialPoint.Y - rect.Height)
            };
        }

        private List<Point> GetPotentialPosingPointsFromRectangle(Rectangle rect)
        {
            var points = new List<Point>();
            if (isFirst)
                points.AddRange(GetPotentialPosingPointsForFirstRect(rect));

            points.Add(new Point(rect.Location.X + rect.Width, rect.Location.Y));
            points.Add(new Point(rect.Location.X + rect.Width, rect.Location.Y + rect.Height));
            points.Add(new Point(rect.Location.X, rect.Location.Y + rect.Height));
            points.Add(rect.Location);

            return points;
        }

        private List<Point> GetPotentialPosingPointsForFirstRect(Rectangle rect)
        {
            return new List<Point>
            {
                new Point(rect.Location.X + rect.Width / 2, rect.Location.Y),
                new Point(rect.Location.X, rect.Location.Y + rect.Height / 2),
                new Point(rect.Location.X + rect.Width / 2, rect.Location.Y + rect.Height),
                new Point(rect.Location.X, rect.Location.Y + rect.Height / 2)
            };
        }
    }
}