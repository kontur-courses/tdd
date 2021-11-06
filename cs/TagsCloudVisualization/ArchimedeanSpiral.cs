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
        /// Текущий угол в радианах
        /// </summary>
        private double _phi;

        public ArchimedeanSpiral(Point center)
        {
            _offSetX = center.X;
            _offSetY = center.Y;
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
                var x = rho * Math.Cos(_phi) + _offSetX;
                var y = rho * Math.Sin(_phi) + _offSetY;
                var point = new Point((int)x, (int)y);

                _phi += 0.01;
                yield return point;
            }
        }
    }
}
