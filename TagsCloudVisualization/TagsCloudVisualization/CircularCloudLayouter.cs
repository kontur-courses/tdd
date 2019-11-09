using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point _center;
        private readonly List<Rectangle> _rectangles = new List<Rectangle>();
        private readonly SortedList<double, HashSet<Point>> _corners = new SortedList<double, HashSet<Point>>();

        public CircularCloudLayouter(Point center)
        {
            this._center = center;
            _corners.Add(0, new HashSet<Point> { center });
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"Width and height should be greater than 0. " +
                                    $"Your width was: {rectangleSize.Width} and height was: {rectangleSize.Height}");

            foreach (var corner in _corners.Values)
                foreach (var rectangle in RectangleGeometry.GetCornerRectangles(rectangleSize, corner))
                {
                    if (_rectangles.Any(rec => rec.IntersectsWith(rectangle))) continue; 
                    _rectangles.Add(rectangle);
                    AddPointsIntoList(rectangle.GetCorners());
                    return rectangle;
                }

            throw new Exception("We can't find the place to add your rectangle, because rectangle location " +
                $"was out of range :(. Your width was: {rectangleSize.Width} and height was: {rectangleSize.Height}");
        }

        private void AddPointsIntoList(IEnumerable<Point> points)
        {
            foreach (var point in points)
            {
                var distance = point.DistanceTo(_center);
                if (_corners.ContainsKey(distance)) _corners[distance].Add(point);
                else _corners.Add(distance, new HashSet<Point>() { point });
            }
        }
    }
}   
