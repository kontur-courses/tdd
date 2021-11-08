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
        private int _offsetX = 0;

        /// <summary>
        /// Смещение по Y к заданному центру
        /// </summary>
        private int _offsetY = 0;

        /// <summary>
        /// Радиус витков
        /// </summary>
        private double radius = 1;

        /// <summary>
        /// Текущий угол в радианах
        /// </summary>
        private double _phi = 0;

        /// <summary>
        /// Создает новый объект спирали Архимеда с центров в точке Point center
        /// </summary>
        /// <param name="center">Центр спирали</param>
        /// 
        public ArchimedeanSpiral() { }

        public ArchimedeanSpiral(Point center)
        {
            _offsetX = center.X;
            _offsetY = center.Y;
        }

        /// <summary>
        /// Выдает дискретные значения спирали от последнего взятого значения до бесконечности!
        /// </summary>
        public IEnumerable<Point> GetDiscretePoints()
        {
            while (true)
            {
                var rho = _phi * radius / (2 * Math.PI);
                var cartesian = CoordinatesConverter.ToCartesian(rho, _phi);
                var point = new Point(cartesian.X + _offsetX, cartesian.Y + _offsetY);

                _phi += 0.01;
                yield return point;
            }
        }
    }

    public class CoordinatesConverter
    {
        public static Point ToCartesian(double rho, double phi)
        {
            return new Point((int)(rho * Math.Cos(phi)), (int)(rho * Math.Sin(phi)));
        }

        public static (double rho, double phi) ToPolar(Point point)
        {
            var rho = Math.Sqrt(point.X * point.X + point.Y * point.Y);
            var phi = Math.Atan(point.Y / point.X);
            return (rho, phi);
        }
    }
}
