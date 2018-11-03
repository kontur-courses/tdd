using System.Collections.Generic;
using System;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private const int SpiralSize = 10;
        private double _currentAngle = Math.PI;
        private Point _currentPoint;
        public readonly List<Rectangle> Rects = new List<Rectangle>();
            
        public CircularCloudLayouter(Point center)
        {
            _currentPoint = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rect = new Rectangle(_currentPoint, rectangleSize);

            _currentAngle = Math.PI;
            while (RectOverlapWithExistRects(rect))
            {
                var r = SpiralSize * _currentAngle;
                var x = (int)Math.Floor(r * Math.Cos(_currentAngle));
                var y = (int)Math.Floor(r * Math.Sin(_currentAngle));
                
                _currentPoint = new Point(x, y);
                _currentAngle += 0.1;
                
                rect = new Rectangle(_currentPoint, rectangleSize);
            }
            
            Rects.Add(rect);

            // CorrectLayout();
            
            return rect;
        }

        private void CorrectLayout()
        {
            foreach (var existRect in Rects)
            {
                var newPos = new Point(0, 0);
                existRect.Pos = newPos;
            }
        }

        private bool RectOverlapWithExistRects(Rectangle rect)
        {
            foreach (var existRect in Rects)
            {
                if (Rectangle.IsOverlap(existRect, rect))
                {
                    return true;
                }
            }

            return false;
        }
    }
}