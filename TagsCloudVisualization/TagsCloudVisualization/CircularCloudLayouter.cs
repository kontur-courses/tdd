using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        readonly Point center;
        private List<Rectangle> rectangles = new List<Rectangle>();
        private SortedList<double, HashSet<Point>> corners = new SortedList<double, HashSet<Point>>();

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            corners.Add(0, new HashSet<Point> { center });
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            foreach (var corner in corners.Values)
                foreach (var rectangle in RectangleGeometry.GetCornerRectangles(rectangleSize, corner))
                {
                    if (rectangles.Any(rec => rec.IntersectsWith(rectangle))) continue; 
                    rectangles.Add(rectangle);
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
                var distance = point.DistanceTo(center);
                if (corners.ContainsKey(distance)) corners[distance].Add(point);
                else corners.Add(distance, new HashSet<Point>() { point });
            }
        }
    }
}   
