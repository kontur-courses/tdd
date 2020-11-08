using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter : ICloudLayouter
    {
        private readonly List<Rectangle> _rectangles;
        private readonly ArchimedeanSpiral _spiral;
        private readonly Point _canvasCenter;

        public CircularCloudLayouter(Point canvasCenter)
        {
            _rectangles = new List<Rectangle>();
            _canvasCenter = canvasCenter;
            _spiral = new ArchimedeanSpiral(_canvasCenter);
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("Width and height of the rectangle must be positive");

            Rectangle rectangle;
            do
            {
                var possiblePoint = _spiral.GetNextPoint();
                rectangle = RectangleExtensions.CreateRectangleFromMiddlePointAndSize(possiblePoint, rectangleSize);
            } while (rectangle.IntersectsWith(_rectangles)); 
            var result = MoveToCanvasCenter(rectangle);
            _rectangles.Add(result);
            return result;
        }

        private Rectangle MoveToCanvasCenter(Rectangle rectangle, int axisStep = 1)
        {
            if (rectangle.GetMiddlePoint().Equals(_canvasCenter))
                return rectangle;

            var currentRectangle = rectangle;
            Rectangle result;

            do
            {
                result = currentRectangle;
                currentRectangle = currentRectangle.MoveOneStepTowardsPoint(_canvasCenter, axisStep);
            } while (!currentRectangle.IntersectsWith(_rectangles));
            
            return result;
        }

    }
}
