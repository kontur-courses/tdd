using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Size pictureSize;
        private readonly IEnumerable<Point> spiralPoints;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private int sumRectangleSquare;

        public CircularCloudLayouter(Point center, Size pictureSize)
        {
            this.pictureSize = pictureSize;
            spiralPoints = new Spiral(center).GetPoints();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckRectangleSize(rectangleSize);

            var rectangle = FindSuitableRectangle(rectangleSize);

            rectangles.Add(rectangle);
            sumRectangleSquare += GeometryHelper.GetSquare(rectangleSize);
            return rectangle;
        }

        private void CheckRectangleSize(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException($"{nameof(rectangleSize)} was incorrect");

            if (sumRectangleSquare + GeometryHelper.GetSquare(rectangleSize) > GeometryHelper.GetSquare(pictureSize))
            {
                throw new ArgumentException("rectangle can not be placed, sum size was too big");
            }
        }

        private Rectangle FindSuitableRectangle(Size rectangleSize)
        {
            return spiralPoints
                .Select(p => GeometryHelper.GetRectangleFromCenterPoint(p, rectangleSize))
                .First(current => !rectangles.Any(r => r.IntersectsWith(current)));
        }
    }
}