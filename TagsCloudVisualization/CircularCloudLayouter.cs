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
        private const double CurveAngleStep = 1;
        private double _currentCurveAngle = 15;

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
                result = GetRectangleByCoordinates(CloudCenter, rectangleSize);
            else
                result = GetRectangleByCoordinates(GetNextRectangleCoordinates(), rectangleSize);
            Rectangles.Add(result);
            return result;
        }

        private Rectangle GetRectangleByCoordinates(Point coordinates, Size rectangleSize)
        {
            var location = new Point(coordinates.X - rectangleSize.Width / 2,
                coordinates.Y - rectangleSize.Height / 2);
            return new Rectangle(location, rectangleSize);
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
