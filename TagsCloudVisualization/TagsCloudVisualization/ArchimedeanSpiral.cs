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
        private int _offsetX;

        /// <summary>
        /// Смещение по Y к заданному центру
        /// </summary>
        private int _offsetY;

        /// <summary>
        /// Радиус витков
        /// </summary>
        private double _a;

        /// <summary>
        /// Текущий угол в радианах
        /// </summary>
        private double _phi;

        /// <summary>
        /// Создает новый объект спирали Архимеда с центров в точке (0,0)
        /// </summary>
        public ArchimedeanSpiral()
        {
            _offsetX = 0;
            _offsetY = 0;
            _a = 1;
            _phi = 0;
        }

        /// <summary>
        /// Создает новый объект спирали Архимеда с центров в точке Point center
        /// </summary>
        /// <param name="center">Центр спирали</param>
        public ArchimedeanSpiral(Point center)
        {
            _offsetX = center.X;
            _offsetY = center.Y;
            _a = 1;
            _phi = 0;
        }

        /// <summary>
        /// Выдает дискретные значения спирали от последнего взятого значения до бесконечности!
        /// </summary>
        public IEnumerable<Point> Slide()
        {
            while (true)
            {
                var rho = _phi * _a / (2 * Math.PI);
                var x = rho * Math.Cos(_phi) + _offsetX;
                var y = rho * Math.Sin(_phi) + _offsetY;
                var point = new Point((int)x, (int)y);

                _phi += 0.01;
                yield return point;
            }
        }
    }
}
