using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public List<Rectangle> Rectangles { get; }
        private SortedSet<Point> corners;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            corners = new SortedSet<Point>(new PointsRadiusComparer(center));
        }

        //private List<Point> GetCorners(Rectangle rectangle)
        //{
        //    return new List<Point>
        //    {
        //        rectangle.Location,
        //        new Point(rectangle.Right, rectangle.Top),
        //        new Point(rectangle.Right, rectangle.Bottom),
        //        new Point(rectangle.Left, rectangle.Bottom)
        //    };
        //}

        private void AddCorners(Rectangle rectangle)
        {
            foreach (var corner in rectangle.GetCorners())
                corners.Add(corner);
        }

        private List<Rectangle> GetRectanglesNearThePoint(Point point, Size size)
        {
            return new List<Rectangle>
            {
                new Rectangle(new Point(point.X, point.Y - size.Height), size),
                new Rectangle(point - size, size),
                new Rectangle(new Point(point.X - size.Width, point.Y), size),
                new Rectangle(point, size)
            };
        }

        private bool IsRectangleIntersectsWithOtherRectangles(Rectangle rectangle)
        {
            return Rectangles.Any(rectangle.IntersectsWith);
        }

        private int GetSuitabilityCoefficient(Rectangle rectangle)
        {
            var corners = rectangle.GetCorners();
            return corners.Sum(point => point.SquaredDistanceTo(Center));
        }

        private Rectangle GetMostSuitableRectangle(List<Rectangle> rectangles)
        {
            var minCoefficient = GetSuitabilityCoefficient(rectangles[0]);
            var mostSuitableRectangle = rectangles[0];
            foreach (var rectangle in rectangles.Skip(1))
            {
                var coefficient = GetSuitabilityCoefficient(rectangle);
                if (coefficient < minCoefficient)
                {
                    minCoefficient = coefficient;
                    mostSuitableRectangle = rectangle;
                }
            }
            return mostSuitableRectangle;
        }

        private Rectangle GetRectangle(Size size)
        {
            foreach (var corner in corners)
            {
                var rectangles = GetRectanglesNearThePoint(corner, size);
                var matchingRectangles = rectangles
                    .Where(rect => !IsRectangleIntersectsWithOtherRectangles(rect))
                    .ToList();
                if (matchingRectangles.Count == 0)
                    continue;
                return GetMostSuitableRectangle(matchingRectangles);
            }
            throw new Exception("There was no suitable place for the rectangle");
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rectangle;
            if (Rectangles.Count == 0)
            {
                var location = Center - rectangleSize / 2;
                rectangle = new Rectangle(location, rectangleSize);
            }
            else
                rectangle = GetRectangle(rectangleSize);

            Rectangles.Add(rectangle);
            AddCorners(rectangle);
            return rectangle;
        }

        public List<Rectangle> PutNextRectangles(List<Size> sizes)
        {
            foreach (var size in sizes)
                PutNextRectangle(size);
            return Rectangles;
        }
    }
}
