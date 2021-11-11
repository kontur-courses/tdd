using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    { 
        public readonly List<Rectangle> Rectangles;
        public readonly Point CloudCenter;
        private const double CurveStartingRadius = 0;
        private double _directionBetweenRoundsCoeff = 1 / (2 * Math.PI);
        private double _curveAngleStep = Math.PI / 10;
        private double _currentCurveAngle = 0.0;

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            CloudCenter = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            CheckRectangleSizeCorrectness(rectangleSize);
            Rectangle result = default(Rectangle);
            var nextRectangleCoordinates = GetNextRectangleCoordinates(rectangleSize);
            result = GetRectangleByCenter(nextRectangleCoordinates, rectangleSize);
            Rectangles.Add(result);
            return result;
        }

        private Point GetNextRectangleCoordinates(Size rectSize)
        {
            var nextRectCenter = GetNextRectCenter(rectSize);
            nextRectCenter = ShiftRectangleToPoint(nextRectCenter,
                rectSize, CloudCenter, 1);
            nextRectCenter = ShiftRectangleToPoint(nextRectCenter,
                rectSize, new Point(nextRectCenter.X, CloudCenter.Y), 0);
            nextRectCenter = ShiftRectangleToPoint(nextRectCenter,
                  rectSize, new Point(CloudCenter.X, nextRectCenter.Y), 0);
            return nextRectCenter;
        }

        private Point GetNextRectCenter(Size rectSize)
        {
            if (Rectangles.Count == 0)
                return GetNextCurvePoint();
            var previousRect = Rectangles.Last();
            var nextRectCenter = GetNextCurvePoint();
            if (Rectangles.Count == 1)
            {
                while (GetRectangleByCenter(nextRectCenter, rectSize).IntersectsWith(previousRect))
                    nextRectCenter = GetNextCurvePoint();
                return nextRectCenter;
            }

            var nextRect = GetRectangleByCenter(nextRectCenter, rectSize);
            while (IfRectIntersectAnyOther(nextRect))
            {
                nextRectCenter = GetNextCurvePoint();
                nextRect = GetRectangleByCenter(nextRectCenter, rectSize);
            }

            return nextRectCenter;
        }

        private Point GetNextCurvePoint()
        {
            _currentCurveAngle += _curveAngleStep;
            if (_curveAngleStep > 0.2)
                _curveAngleStep -= 0.002;
            return new Point(Convert.ToInt32((CurveStartingRadius + _directionBetweenRoundsCoeff * _currentCurveAngle) * Math.Cos(_currentCurveAngle)) + CloudCenter.X,
                Convert.ToInt32((CurveStartingRadius + _directionBetweenRoundsCoeff * _currentCurveAngle) * Math.Sin(_currentCurveAngle)) + CloudCenter.Y);
        }

        private Rectangle GetRectangleByCenter(Point centerCoords, Size rectangleSize)
        {
            var location = new Point(centerCoords.X - rectangleSize.Width / 2,
                centerCoords.Y - rectangleSize.Height / 2);
            return new Rectangle(location, rectangleSize);
        }

        private void CheckRectangleSizeCorrectness(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException();
        }

        private bool IfRectIntersectAnyOther(Rectangle rect)
            => Rectangles.Any(r => r != rect && rect.IntersectsWith(r));

        private Point ShiftRectangleToPoint(Point rectCenter, Size rectSize,
            Point shiftDirection, int mod)
        {
            var leftBorder = shiftDirection;
            var rightBorder = rectCenter;
            rectSize.Width += mod;
            rectSize.Height += mod;
            var vectLen = new Vector(leftBorder, rightBorder).GetLength();
            var eps = 2;
            while (vectLen > eps)
            {
                var middle = new Point(
                    (leftBorder.X + rightBorder.X) / 2,
                    (leftBorder.Y + rightBorder.Y) / 2);
                var pos = GetRectangleByCenter(middle, rectSize);
                if (IfRectIntersectAnyOther(pos))
                    leftBorder = middle;
                else
                    rightBorder = middle;
                vectLen = new Vector(leftBorder, rightBorder).GetLength();
            }
            return rightBorder;
        }
    }
}
