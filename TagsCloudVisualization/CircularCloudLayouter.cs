using System;
using System.Drawing;
using System.Collections.Generic;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    { 
        public readonly List<Rectangle> Rectangles;
        public readonly Point CloudCenter;
        private const double A = 15;
        private const double B = 0.015;
        private const double CurveAngleStep = 0.12;
        private double _currentCurveAngle = 70;

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            CloudCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckRectangleSizeCorrectness(rectangleSize);
            var result = new Rectangle();
            if (Rectangles.Count == 0)
            {
                result.Location = new Point(-1 * rectangleSize.Width / 2, rectangleSize.Height / 2);
                Rectangles.Add(result);
            }
            return result;
        }

        private Point GetNextRectangleCoordinates()
        {
            var x = Convert.ToInt32(A * Math.Pow(Math.E, B * _currentCurveAngle) * Math.Cos(_currentCurveAngle));
            var y = Convert.ToInt32(A * Math.Pow(Math.E, B * _currentCurveAngle) * Math.Sin(_currentCurveAngle));
            _currentCurveAngle += CurveAngleStep;
            return new Point(x, y);

        }

        private void CheckRectangleSizeCorrectness(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
        }
    }
}
