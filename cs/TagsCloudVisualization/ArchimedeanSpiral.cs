using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    /// <summary>
    /// Спираль Архимеда
    /// </summary>
    internal class ArchimedeanSpiral
    {
        /// <summary>
        /// Смещение по X к заданному центру
        /// </summary>
        private int _offSetX;
        /// <summary>
        /// Смещение по Y к заданному центру
        /// </summary>
        private int _offSetY;
        /// <summary>
        /// Радиус витков
        /// </summary>
        private double _a;
        /// <summary>
        /// Шаг выборки дискретных точек на спирали в радианах
        /// </summary>
        private double _delta;
        /// <summary>
        /// Начальный угол в радианах
        /// </summary>
        private double _phi;

        public ArchimedeanSpiral(Point center)
        {
            _offSetX = center.X;
            _offSetY = center.Y;
            _a = 10;
            _phi = Math.PI * 6;
            _delta = 1.6; // PI / 2 = 1.57
        }

        /// <summary>
        /// Скользит по спирали в диапазоне +- 1 радиан
        /// </summary>
        /// <returns>Дискретные значения из окрестности следующей точки в диапазоне +- 1 радиан с точностью 10^-2</returns>
        public HashSet<Point> Slide()
        {
            var points = new HashSet<Point>();
            for (double phi = _phi - 1; phi <= _phi + 1; phi += 0.01)
            {
                var rho = phi * _a / (2 * Math.PI);
                var x = rho * Math.Cos(phi) + _offSetX;
                var y = rho * Math.Sin(phi) + _offSetY;
                points.Add(new Point((int)x, (int)y));
            }
            _phi += _delta;
            return points;
        }
    }
}
