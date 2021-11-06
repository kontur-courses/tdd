using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization.Extensions
{
    public static class RectangleExtensions
    {
        /// <summary>
        /// Находит расстояние от точки внутри прямоугольника до каждой его стороны
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="point">Точка внутри прямоугольника</param>
        /// <returns>Возвращает расстояния в порядке left, top, right, bottom (все расстояния положительные)</returns>
        public static List<int> GetDistancesToPoint(this Rectangle rect, Point point)
        {
            var left = point.X - rect.Left;
            var top = point.Y - rect.Top;
            var right = rect.Right - point.X;
            var bottom = rect.Bottom - point.Y;

            var distances = new List<int> { left, top, right, bottom };
            if (distances.Any(d => d < 0))
                throw new ArgumentException("Точка расположена вне прямоугольника");
            return distances;
        }

        /// <summary>
        /// Находит координаты центральной точки прямоугольника
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <returns>объект Point - центр прямоугольника</returns>
        public static Point GetCenter(this Rectangle rect)
        {
            return new Point(rect.Left + rect.Width / 2, rect.Top + rect.Height / 2);
        }

        /// <summary>
        /// Находит координату верхнего левого угла прямоугольника по координатам его центра
        /// </summary>
        /// <param name="rect">прямоугольник</param>
        /// <param name="rectCenter">центральная точка прямоугольника</param>
        /// <returns>Координата левого верхнего угла прямоугольника</returns>
        public static Point GetLocationFromCenter(this Rectangle rect, Point rectCenter)
        {
            return new Point(rectCenter.X - rect.Width / 2, rectCenter.Y - rect.Height / 2);
        }

        /// <summary>
        /// Возвращает новый прямоугольник полученный путем пересечения двух прямоугольников
        /// </summary>
        public static Rectangle GetIntersect(this Rectangle first, Rectangle second)
        {
            first.Intersect(second);
            return first;
        }

        /// <summary>
        /// Находит расстояние от точки внутри прямоугольника до каждого его угла
        /// </summary>
        /// <param name="rect">Прямоугольник</param>
        /// <param name="point">Точка внутри прямоугольника</param>
        /// <returns>Возвращает расстояния в порядке lelft-top, right-top, right-bottom, left-bottom (все расстояния положительные)</returns>
        public static IEnumerable<double> GetCornerDistancesToPoint(this Rectangle rect, Point point)
        {
            var distanses = GetDistancesToPoint(rect, point);
            distanses.Add(distanses.First());
            return distanses
                .Zip(distanses.Skip(1), (first, second) => (first, second))
                .Select(t => Math.Sqrt(t.first*t.first + t.second*t.second));
        }
    }
}
