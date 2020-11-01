using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using TagsCloudVisualization.Extensions;

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
            var rectangle = new Rectangle();
            foreach (var point in spiral.GetSpiralPoints())
            {
                rectangle.Location = point;
                rectangle.Size = rectangleSize;
                if (!rectangle.IsIntersectsWith(rectangles))
                    break;
            }
            var compactRectangle = CompactRectangle(rectangle);
            rectangles.Add(compactRectangle);
            return compactRectangle;
        }

        private Rectangle CompactRectangle(Rectangle rectangle)
        {
            var targetVector = new TargetVector(center, rectangle.Location);
            foreach (var delta in targetVector.GetPartialDelta())
            {
                var newRectangle = rectangle.MoveOnTheDelta(delta);
                if (newRectangle.IsIntersectsWith(rectangles))
                    break;
                rectangle = newRectangle;
            }
            return rectangle;
        }
    }
}
