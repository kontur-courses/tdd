using System.Collections.Generic;
using System.Drawing;
using TagsCloudVisualization.Extensions;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; }
        Point center;
        Spiral spiral;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            Rectangles = new List<Rectangle>();
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
                if (!rectangle.IsIntersectsWith(Rectangles))
                    break;
            }
            var compactRectangle = CompactRectangle(rectangle);
            Rectangles.Add(compactRectangle);
            return compactRectangle;
        }

        private Rectangle CompactRectangle(Rectangle rectangle)
        {
            var targetVector = new TargetVector(center, rectangle.Location);
            foreach (var delta in targetVector.GetPartialDelta())
            {
                var newRectangle = rectangle.MoveOnTheDelta(delta);
                if (newRectangle.IsIntersectsWith(Rectangles))
                    break;
                rectangle = newRectangle;
            }
            return rectangle;
        }

        public Size GetLayoutSize()
        {
            var width = 0;
            var height = 0;
            foreach (var rect in Rectangles)
            {
                width += center.X - rect.X + rect.Width;
                height += center.Y - rect.Y + rect.Height;
            }
            return new Size(width, height);
        }
    }
}
