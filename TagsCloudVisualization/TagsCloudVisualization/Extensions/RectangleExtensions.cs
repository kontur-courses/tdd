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
        /// Возвращает новый прямоугольник полученный путем пересечения двух прямоугольников
        /// </summary>
        public static Rectangle GetIntersection(this Rectangle first, Rectangle second)
        {
            first.Intersect(second);
            return first;
        }
    }
}
