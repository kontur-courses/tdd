using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class PlanePart
    {
        private readonly Point center;
        private readonly int rectangleMargin;
        private readonly HashSet<Rectangle> rectangles;
        private readonly PartType partType;

        public PlanePart(Point center, PartType partType, int rectangleMargin)
        {
            this.center = center;
            this.partType = partType;
            this.rectangleMargin = rectangleMargin;
            rectangles = new HashSet<Rectangle>();
        }

        public Point GetBestLocation(Size rectangleSize)
        {
            // Добавлять прямоугольники можно по горизонтали и по вертикали, среди результатов беру лучший
            var (horizontalDistance, horizontalPoint) = GetLocation(rectangleSize, true);
            var (verticalDistance, verticalPoint) = GetLocation(rectangleSize, false);

            return horizontalDistance < verticalDistance ? horizontalPoint : verticalPoint;
        }

        private Tuple<double, Point> GetLocation(Size rectangleSize, bool isHorizontal)
        {
            var shorterDistance = double.MaxValue;
            var bestLocation = new Point();
            foreach (var rect in rectangles)
            {
                // В зависимости от типа части, и тому, куда в какую сторону будет добавляться прямоугольник, нужно
                // посчитать отступ от левого верхнего угла того прямоугольника, относительно которого будем размещать
                // текущий
                var offset = isHorizontal
                    ? GetHorizontalOffset(rectangleSize, rect)
                    : GetVerticalOffset(rectangleSize, rect);
                var location = new Point(rect.X + offset.X, rect.Y + offset.Y);
                var distance = CircularCloudLayouter.CalculateDistance(location.X + rectangleSize.Width / 2.0,
                    location.Y + rectangleSize.Height / 2.0, center.X, center.Y);

                if (shorterDistance > distance &&
                    !IntersectWithOtherRectangles(new Rectangle {Size = rectangleSize, Location = location}))
                {
                    shorterDistance = distance;
                    bestLocation = location;
                }
            }

            return new Tuple<double, Point>(shorterDistance, bestLocation);
        }

        private bool IntersectWithOtherRectangles(Rectangle rectangle)
        {
            return rectangles.Any(rect => !Rectangle.Intersect(rect, rectangle).Equals(Rectangle.Empty));
        }

        private Point GetVerticalOffset(Size rectangleSize, Rectangle previous)
        {
            return partType switch
            {
                PartType.Top => new Point(0, previous.Height + rectangleMargin),
                PartType.Right => new Point(0, -rectangleSize.Height - rectangleMargin),
                PartType.Bottom => new Point(previous.Width - rectangleSize.Width,
                    -rectangleSize.Height - rectangleMargin),
                PartType.Left => new Point(previous.Width - rectangleSize.Width,
                    previous.Height + rectangleMargin),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Point GetHorizontalOffset(Size rectangleSize, Rectangle previous)
        {
            return partType switch
            {
                PartType.Top => new Point(previous.Width + rectangleMargin, 0),
                PartType.Right => new Point(previous.Width + rectangleMargin,
                    previous.Height - rectangleSize.Height),
                PartType.Bottom => new Point(-rectangleSize.Width - rectangleMargin,
                    previous.Height - rectangleSize.Height),
                PartType.Left => new Point(-rectangleSize.Width - rectangleMargin, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void AddRectangle(Rectangle rectangle)
        {
            rectangles.Add(rectangle);
        }
    }
}