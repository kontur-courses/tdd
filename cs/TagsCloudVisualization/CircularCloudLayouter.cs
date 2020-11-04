using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private readonly ArchimedeanSpiral _spiral;
        private readonly Point _center;

        public CircularCloudLayouter(Point center)
        {
            _rectangles = new List<Rectangle>();
            _center = center;
            _spiral = new ArchimedeanSpiral(_center);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of the rectangle must be non-negative");
            var result = CreateRectangle(_spiral.GetNextPoint(), rectangleSize);
            while (result.IntersectsWith(_rectangles))
                result = CreateRectangle(_spiral.GetNextPoint(), rectangleSize);
            result = MoveToCenter(result);
            _rectangles.Add(result);
            return result;
        }

        private Rectangle MoveToCenter(Rectangle rectangle)
        {
            var correctMiddlePoint = rectangle.GetMiddlePoint();
            var currentMiddlePoint = rectangle.GetMiddlePoint();
            while (currentMiddlePoint.X != _center.X && currentMiddlePoint.Y != _center.Y)
            {
                var offsetX = currentMiddlePoint.X < _center.X ? 1 : -1;
                var offsetY = currentMiddlePoint.Y < _center.Y ? 1 : -1;
                currentMiddlePoint.Offset(offsetX, offsetY);
                var tempRectangle = CreateRectangle(currentMiddlePoint, rectangle.Size);
                if (!tempRectangle.IntersectsWith(_rectangles))
                    correctMiddlePoint = currentMiddlePoint;
            }

            return CreateRectangle(correctMiddlePoint, rectangle.Size);
        }

        /*
         * Не очень красиво, что метод, напрямую не относящийся к логике, здесь. 
         * Хотелось бы сделать его методом расширения, 
         * чтобы можно было вызывать Rectangle.CreateRectangle(...). Как лучше?
         */
        private Rectangle CreateRectangle(Point centerOfRectangle, Size rectangleSize)
        {
            var x = centerOfRectangle.X - rectangleSize.Width / 2;
            var y = centerOfRectangle.Y - rectangleSize.Height / 2;
            return new Rectangle(new Point(x, y), rectangleSize);
        }
    }
}
