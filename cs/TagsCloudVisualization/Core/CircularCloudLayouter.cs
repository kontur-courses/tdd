using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        private readonly List<Rectangle> rectangles;
        private readonly ArchimedeanSpiral spiral;

        public int MaxX { get; private set; }
        public int MaxY { get; private set; }

        public IEnumerable<Rectangle> Rectangles => rectangles;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            spiral = new ArchimedeanSpiral(this.center);
        }

        /// <summary>
        /// Определяет пересекается ли rectangle с уже добавленными прямоугольниками
        /// </summary>
        /// <param name="rectangle">Проверяемый прямоугольник</param>
        /// <returns>true, если пересекается, иначе false</returns>
        public bool DoesItIntersectWithSome(Rectangle rectangle) => rectangles.Any(r => r.IntersectsWith(rectangle));

        /// <summary>
        /// Добавляет прямоугольник в облако тегов.
        /// </summary>
        /// <param name="rectangleSize">Размер прямоугольника</param>
        /// <returns>Добавленный прямоугольник</returns>
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();
            foreach (var point in spiral.GetPoints(50))
            {
                rectangle = new Rectangle(point, rectangleSize);
                if (!DoesItIntersectWithSome(rectangle))
                    break;
            }

            if (rectangles.Count > 0)
                rectangle = ShiftToCenter(rectangle);
            UpdateMaximumValues(rectangle);

            rectangles.Add(rectangle);
            return rectangle;
        }

        /// <summary>
        /// Возвращает нормированный вектор, который мы должны применить к second, чтобы добраться до first
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="axis">Ось</param>
        /// <returns>Нормированный вектор</returns>
        private Point GetOffset(Point first, Point second, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return first.X - second.X > 0 ? new Point(1, 0) : new Point(-1, 0);
                case Axis.Y:
                    return first.Y - second.Y > 0 ? new Point(0, 1) : new Point(0, -1);
                default:
                    throw new ArgumentException("Axis can only be X or Y");
            }
        }

        /// <summary>
        /// Сдвигает прямоугольник до упора к остальным примоугольникам по заданной оси.
        /// </summary>
        /// <param name="rectangle">Прямоугольник, который сдвигаем</param>
        /// <param name="axis">Ось.</param>
        /// <returns>Сдвинутый прямоугольник</returns>
        private Rectangle ShiftToCenterAlongOneAxis(Rectangle rectangle, Axis axis)
        {
            var offset = GetOffset(center, rectangle.Location, axis);
            var oldRectangle = rectangle;
            var axisCenterValue = center.SelectCoordinatePointAlongAxis(axis);
            var axisRectangleValue = rectangle.Location.SelectCoordinatePointAlongAxis(axis);

            while (!DoesItIntersectWithSome(rectangle) && axisCenterValue != axisRectangleValue)
            {
                oldRectangle = rectangle;
                var newLocation = rectangle.Location.Add(offset);
                rectangle = new Rectangle(newLocation, rectangle.Size);
                axisRectangleValue = rectangle.Location.SelectCoordinatePointAlongAxis(axis);
            }
            return oldRectangle;
        }

        /// <summary>
        /// Сдвигаем прямоугольник к центру по всем осям
        /// </summary>
        /// <param name="rectangle">Прямоугольник, который сдвигаем</param>
        /// <returns>Сдвинутый прямоугольник</returns>
        private Rectangle ShiftToCenter(Rectangle rectangle)
        {
            rectangle = ShiftToCenterAlongOneAxis(rectangle, Axis.X);
            return ShiftToCenterAlongOneAxis(rectangle, Axis.Y);
        }

        /// <summary>
        /// Обновляем значения максимальных значений по осям
        /// </summary>
        /// <param name="rectangle">Прямоугольник, который добавили</param>
        private void UpdateMaximumValues(Rectangle rectangle)
        {
            MaxX = Math.Max(MaxX, rectangle.Right);
            MaxY = Math.Max(MaxY, rectangle.Bottom);
        }
    }
}