using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Rectangle pictureRectangle;
        private readonly IEnumerable<Point> spiralPoints;
        private readonly List<Rectangle> rectangles = new List<Rectangle>();

        public CircularCloudLayouter(Point center, Size pictureSize)
        {
            if (CheckIfSizeIsIncorrect(pictureSize))
            {
                throw new ArgumentException($"{nameof(pictureSize)} was incorrect");
            }

            pictureRectangle = GeometryHelper.GetRectangleFromCenterPoint(center, pictureSize);
            spiralPoints = new Spiral(center).GetPoints();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckRectangleSize(rectangleSize);

            var rectangle = FindSuitableRectangle(rectangleSize);

            HandleFoundRectangle(rectangle);
            return rectangle;
        }

        private void HandleFoundRectangle(Rectangle rectangle)
        {
            if (!pictureRectangle.Contains(rectangle))
            {
                throw new ArgumentException("rectangle can not be placed in picture");
            }
            rectangles.Add(rectangle);
        }

        private void CheckRectangleSize(Size rectangleSize)
        {
            if (CheckIfSizeIsIncorrect(rectangleSize))
                throw new ArgumentException($"{nameof(rectangleSize)} was incorrect");
        }

        private Rectangle FindSuitableRectangle(Size rectangleSize)
        {
            return spiralPoints
                .Select(p => GeometryHelper.GetRectangleFromCenterPoint(p, rectangleSize))
                .First(current => !rectangles.Any(r => r.IntersectsWith(current)));
        }

        private bool CheckIfSizeIsIncorrect(Size size) => size.Width <= 0 || size.Height <= 0;
    }
}