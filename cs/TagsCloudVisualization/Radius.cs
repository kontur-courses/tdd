using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Представляет собой радиус вектор из точки center до точки end
    /// </summary>
    internal class Radius
    {
        private Point _center;
        private Point _end;        

        /// <param name="center">Точка из которой проведен вектов</param>
        /// <param name="end">Конечная точка вектора</param>
        public Radius(Point center, Point end)
        {
            _center = center;
            _end = end;
        }

        public HashSet<Point> LinSpaceToCenter(double delta)
        {
            var points = new HashSet<Point>();
            var xOffset = _end.X - _center.X;
            var yOffset = _end.Y - _center.Y;

            for (double l = 1; l > 0; l -= delta)
            {
                var x = (int)(_center.X + xOffset * l);
                var y = (int)(_center.Y + yOffset * l);
                points.Add(new Point(x, y));
            }
            return points;
        }

        /// <summary>
        /// Возвращает дискретные значения бесконечного радиус-вектора начиная с длины Start
        /// </summary>
        /// <param name="start">Начальная длина радиус вектора</param>
        /// <returns>Дискретные значения от start до бесконечности!</returns>
        public IEnumerable<Point> LinSpaceFromCenter(double start)
        {
            var delta = 0.01;
            var xOffset = _end.X - _center.X;
            var yOffset = _end.Y - _center.Y;
            double l = Math.Abs(xOffset) > Math.Abs(yOffset) ? start / Math.Abs(xOffset) : start / Math.Abs(yOffset);
            //if (xOffset == 0 && yOffset == 0)
            //{
            //    xOffset = 1;
            //    yOffset = 1;
            //    l = start;
            //}
            while (true)
            {
                var x = (int)Math.Floor(_center.X + xOffset * l);
                var y = (int)Math.Floor(_center.Y + yOffset * l);
                if (x == int.MinValue || y == int.MinValue)
                    throw new Exception();
                yield return (new Point(x, y));
                l += delta;
            }
        }

        /// <summary>
        /// Длина отреска
        /// </summary>
        public static double Length(Point a, Point b) =>
            Math.Sqrt(Math.Pow(Math.Abs(a.X - b.X), 2) + Math.Pow(Math.Abs(a.Y - b.Y), 2));
    }
}
