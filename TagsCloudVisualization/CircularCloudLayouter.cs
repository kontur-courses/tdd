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
        private const double CurveRoundsWidthCoeff = 15;
        private const double DirectionBetweenRoundsCoeff = 0.015;
        private const double MoveCoeff = 0.05;
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
            var result = default(Rectangle);
            if (Rectangles.Count == 0)
                result = GetRectangleByCoordinates(CloudCenter, rectangleSize);
            else
            {
                var nextRectangleCoordinates = GetNextRectangleCoordinates();
                var movedRectangleCoordinates = MoveRectCoordsFarFromCenter(nextRectangleCoordinates);
                movedRectangleCoordinates = ShiftRectangleToPoint(movedRectangleCoordinates,
                    rectangleSize, CloudCenter);
                result = GetRectangleByCoordinates(movedRectangleCoordinates, rectangleSize);
            }
            Rectangles.Add(result);
            return result;
        }

        private Point ShiftRectangleToPoint(Point rectCenter, Size rectSize,
            Point shiftDirection)
        {
            var leftBorder = shiftDirection;
            var rightBorder = rectCenter;
            rectSize.Width += 4;
            rectSize.Height += 4;
            var vectLen = new Vector(leftBorder, rightBorder).GetLength();
            var eps = 4;
            while (vectLen > eps)
            {
                var middle = new Point(
                    (leftBorder.X + rightBorder.X) / 2,
                    (leftBorder.Y + rightBorder.Y) / 2);
                var pos = GetRectangleByCoordinates(middle, rectSize);
                if (IfRectIntersectAnyOther(pos))
                    leftBorder = middle;
                else
                    rightBorder = middle;
                vectLen = new Vector(leftBorder, rightBorder).GetLength();
            }
            return rightBorder;
        }

        private bool IfRectIntersectAnyOther(Rectangle rect)
            => Rectangles.Any(r => r != rect && rect.IntersectsWith(r));

        private Point MoveRectCoordsFarFromCenter(Point nextRectCoord)
        {
            var movedX = (nextRectCoord.X * (1 + MoveCoeff) - CloudCenter.X) / MoveCoeff;
            var movedY = (nextRectCoord.Y * (1 + MoveCoeff) - CloudCenter.Y) / MoveCoeff;
            return new Point((int) movedX, (int) movedY);
        }

        private Point GetNextRectangleCoordinates()
        {
            var nextRectangleCoordinates = GetNextCurveCoordinates();
            var lastAddedRectangle = Rectangles.Last();
            while (lastAddedRectangle.Contains(nextRectangleCoordinates))
                _currentCurveAngle += CurveAngleStep;
            return nextRectangleCoordinates;
        }

        private Rectangle GetRectangleByCoordinates(Point coordinates, Size rectangleSize)
        {
            var location = new Point(coordinates.X - rectangleSize.Width / 2,
                coordinates.Y - rectangleSize.Height / 2);
            return new Rectangle(location, rectangleSize);
        }

        private Point GetNextCurveCoordinates()
        {
            var x = Convert.ToInt32(CurveRoundsWidthCoeff * 
                Math.Pow(Math.E, DirectionBetweenRoundsCoeff * _currentCurveAngle) 
                * Math.Cos(_currentCurveAngle));
            var y = Convert.ToInt32(CurveRoundsWidthCoeff *
                Math.Pow(Math.E, DirectionBetweenRoundsCoeff * _currentCurveAngle) 
                * Math.Sin(_currentCurveAngle));
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
