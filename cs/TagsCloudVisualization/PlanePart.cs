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
        private HashSet<Rectangle> extremeRectanglesVertically;
        private HashSet<Rectangle> extremeRectanglesHorizontally;
        private bool hasEmptyRectangle;
        private readonly PartType partType;

        public PlanePart(Point center, PartType partType, int rectangleMargin)
        {
            this.center = center;
            this.partType = partType;
            this.rectangleMargin = rectangleMargin;
            extremeRectanglesVertically = new HashSet<Rectangle>();
            extremeRectanglesHorizontally = new HashSet<Rectangle>();
        }

        public PlacementInfo GetBestLocation(Size rectangleSize)
        {
            // Добавлять прямоугольники можно по горизонтали и по вертикали, среди результатов беру лучший
            var (horizontalDistance, horizontalPoint) = GetLocation(extremeRectanglesHorizontally, rectangleSize, true);
            var (verticalDistance, verticalPoint) = GetLocation(extremeRectanglesVertically, rectangleSize, false);

            return horizontalDistance < verticalDistance
                ? new PlacementInfo(false, rectangleSize, horizontalPoint, horizontalDistance)
                : new PlacementInfo(true, rectangleSize, verticalPoint, verticalDistance);
        }

        private Tuple<double, Point> GetLocation(IEnumerable<Rectangle> rectangles, Size rectangleSize,
            bool isHorizontal)
        {
            // Считаю расстояние и координаты относительно внешних прямоугольников, внутренние(закрытые другими) нас не
            // интересуют
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
            return extremeRectanglesVertically.Any(rect => rect.IntersectsWith(rectangle)) ||
                   extremeRectanglesHorizontally.Any(rect => rect.IntersectsWith(rectangle));
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

        public void AddRectangle(PlacementInfo placementInfo)
        {
            if (extremeRectanglesVertically.Count == 0)
            {
                // Добавляю пустой прямоугольник
                extremeRectanglesVertically.Add(placementInfo.Rectangle);
                extremeRectanglesHorizontally.Add(placementInfo.Rectangle);
                hasEmptyRectangle = true;
                return;
            }

            if (hasEmptyRectangle)
            {
                // Костыль, появившийся вследствие костыля с пустыми прямоугольниками
                // Если присутствует только пустой прямоугольник, заменяю его на нормальный
                extremeRectanglesVertically = new HashSet<Rectangle> {placementInfo.Rectangle};
                extremeRectanglesHorizontally = new HashSet<Rectangle> {placementInfo.Rectangle};
                hasEmptyRectangle = false;
                return;
            }

            var newExtremeRectangles = new HashSet<Rectangle>();
            // В зависомомсти от того, куда добавляется прямоугольник, выбираем соответствубщее множество
            var extremeRectangles =
                placementInfo.ToHorizontal ? extremeRectanglesHorizontally : extremeRectanglesVertically;
            foreach (var rectangle in extremeRectangles)
            {
                // Если прямоугольник, который мы хотим добавить, перекрывает уже существующий, то существующий
                // перестает быть крайним, и его добавлять в обновленное множество крайних не нужно
                if (placementInfo.ToHorizontal && placementInfo.Rectangle.OnSameHeight(rectangle) ||
                    placementInfo.ToVertical && placementInfo.Rectangle.OnSameLatitude(rectangle))
                    newExtremeRectangles.Add(placementInfo.Rectangle);
                else
                    newExtremeRectangles.Add(rectangle);
            }

            // Добавляем новый прямоугольник
            newExtremeRectangles.Add(placementInfo.Rectangle);

            // Обновляем множество крайних
            if (placementInfo.ToHorizontal)
                extremeRectanglesHorizontally = newExtremeRectangles;
            else
                extremeRectanglesVertically = newExtremeRectangles;

            if (placementInfo.ToVertical)
                extremeRectanglesHorizontally.Add(placementInfo.Rectangle);

            if (placementInfo.ToHorizontal)
                extremeRectanglesHorizontally.Add(placementInfo.Rectangle);
        }
    }

    public static class RectangleExtensions
    {
        public static bool OnSameLatitude(this Rectangle rect1, Rectangle rect2)
        {
            return rect1.X <= rect2.X + rect2.Width && rect1.X + rect1.Width >= rect2.X;
        }

        public static bool OnSameHeight(this Rectangle rect1, Rectangle rect2)
        {
            return rect1.Y <= rect2.Y + rect2.Height && rect1.Y + rect1.Height >= rect2.Y;
        }
    }
}