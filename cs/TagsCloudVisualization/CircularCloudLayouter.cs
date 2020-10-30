using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        Point center;
        List<Rectangle> rectangles;
        Spiral spiral;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            spiral = new Spiral(center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            /// Идея алгоритма:
            /// изначально в каждой точке спирали  spiral.GetSpiralPoints
            /// буду пытаться разместить прямоугольник 
            /// если он в какой-то точке не пересекает остальные из rectangles
            ///     то заканчиваю перебор точек и понемногу двигаю прямоугольник по вектору center - rectangle.Location
            /// если во время перемещения прямоугольника он начинает пересекать остальные, 
            ///     то заканчиваю перемещение и возвращаю полученный прямоугольник

            return new Rectangle();
        }
    }
}
