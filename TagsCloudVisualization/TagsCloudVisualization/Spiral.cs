using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public abstract class Spiral
    {
        /// <summary>
        /// Смещение по X к заданному центру
        /// </summary>
        protected int OffsetX;

        /// <summary>
        /// Смещение по Y к заданному центру
        /// </summary>
        protected int OffsetY;

        /// <summary>
        /// Радиус витков
        /// </summary>
        protected double Radius;

        /// <summary>
        /// Текущий угол в радианах
        /// </summary>
        protected double Phi;

        /// <summary>
        /// Создает новый объект спирали с центром в точке (0,0)
        /// </summary>
        public Spiral() : this(Point.Empty)
        {
        }

        /// <summary>
        /// Создает новый объект спирали с центром в точке Point center
        /// </summary>
        /// <param name="center">Центр спирали</param>
        /// 
        public Spiral(Point center)
        {
            OffsetX = center.X;
            OffsetY = center.Y;
            Radius = 1;
            Phi = 0;
        }

        /// <summary>
        /// Выдает дискретные значения спирали от последнего взятого значения до бесконечности!
        /// </summary>
        public abstract IEnumerable<Point> GetDiscretePoints(double deltaAngle = 0.01);

        public void SetCenter(Point center)
        {
            OffsetX = center.X;
            OffsetY = center.Y;
        }
    }
}
