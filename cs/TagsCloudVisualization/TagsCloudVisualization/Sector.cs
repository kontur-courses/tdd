using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class Sector
    {
        private readonly List<Point> _points;
        private readonly Point _center;
        private  int _xCoefficient;
        private  int _yCoefficient;
        private  int _alphaCoefficient;
        private  double _alphaShift;

        public Sector(Quadrant quadrant, Point center)
        {
            _points = new List<Point>();
            _center = center;
            AddPoint(0, 0);

            InitCoefficients(quadrant);
        }
        private void InitCoefficients(Quadrant quadrant)
        {
            switch (quadrant)
            {
                case Quadrant.First:
                    _xCoefficient = 1;
                    _yCoefficient = 1;
                    _alphaCoefficient = 1;
                    _alphaShift = 0;
                    break;
                case Quadrant.Second:
                    _xCoefficient = -1;
                    _yCoefficient = 1;
                    _alphaCoefficient = -1;
                    _alphaShift = Math.PI;
                    break;
                case Quadrant.Third:
                    _xCoefficient = -1;
                    _yCoefficient = -1;
                    _alphaCoefficient = 1;
                    _alphaShift = -Math.PI;
                    break;
                case Quadrant.Fourth:
                    _xCoefficient = 1;
                    _yCoefficient = -1;
                    _alphaCoefficient = -1;
                    _alphaShift = 0;
                    break;
            }
        }

        public Rectangle PlaceRectangle(double direction, Size rectangleSize)
        {
            direction = MakeDirectionRelative(direction);
            var availablePoint = FindAvailablePoint(direction);
            RecalculatePointsInSector(availablePoint, rectangleSize);

            var rectangleLocation = FormatPoint(availablePoint, rectangleSize);

            return new Rectangle(rectangleLocation, rectangleSize);
        }

        private double MakeDirectionRelative(double direction)
        {
            return direction * _alphaCoefficient + _alphaShift;
        }

        private Point FindAvailablePoint(double direction)
        {
            for (int index = 0; index < PointsNumber(); index++)
            {
                var point = _points[index];
                if (IsPointOnDirectLine(point.X, point.Y, direction))
                    return point;

                if (IsPointUnderDirectLine(point.X, point.Y, direction))
                    return _points[index - 1].X == point.X ? point : _points[index - 1];
            }

            return new Point(0, 0);
        }

        public void RecalculatePointsInSector(Point downLeftPoint, Size rectangleSize)
        {
            var maxX = downLeftPoint.X + rectangleSize.Width;
            var maxY = downLeftPoint.Y + rectangleSize.Height;

            var rangeToRemove = FindPointsRangeToRemove(_points, maxX, maxY);

            RemovePointsUnderNewRectangle(rangeToRemove);
            InsertNewPointsCreatedByRectangle(rangeToRemove.Item1, maxX, maxY);
        }

        public void InsertNewPointsCreatedByRectangle(int index, int maxX, int maxY)
        {
            // Косяк. По факту от 1 до 3 новых точек. 3 в данном случае.
            _points.Insert(index, new Point(maxX, maxY));
        }

        public static Tuple<int, int> FindPointsRangeToRemove(List<Point> points, int maxX, int maxY)
        {
            var rangeToRemove = new List<int>();

            for (int index = 0; index < points.Count; index++)
            {
                var point = points[index];
                if (point.X <= maxX && point.Y <= maxY)
                    rangeToRemove.Add(index);
            }

            return new Tuple<int, int>(rangeToRemove[0], rangeToRemove[rangeToRemove.Count - 1]);
        }
        
        public void RemovePointsUnderNewRectangle(Tuple<int, int> rangeToRemove)
        {
            _points.RemoveRange(rangeToRemove.Item1, rangeToRemove.Item2);
        }

        private Point FormatPoint(Point rectangleLocation, Size rectangleSize)
        {
            rectangleLocation = FindRectangleCenter(rectangleLocation, rectangleSize);
            rectangleLocation = MakeCoordinatesAbsolute(rectangleLocation);

            return rectangleLocation;
        }

        private static Point FindRectangleCenter(Point downLeftPoint, Size rectangleSize)
        {
            var centerX = downLeftPoint.X + rectangleSize.Width / 2;
            var centerY = downLeftPoint.Y + rectangleSize.Height / 2;

            return new Point(centerX, centerY);
        }

        private Point MakeCoordinatesAbsolute(Point rectangleLocation)
        {
            rectangleLocation.X += _center.X;
            rectangleLocation.Y += _center.Y;

            rectangleLocation.X *= _xCoefficient;
            rectangleLocation.Y *= _yCoefficient;

            return rectangleLocation;
        }

        public static bool IsPointOnDirectLine(int x, int y, double direction)
        {
            return y == x * direction;
        }

        public static bool IsPointUnderDirectLine(int x, int y, double direction)
        {
            return y < x * direction;
        }

        private void AddPoint(int x, int y)
        {
            _points.Add(new Point(x, y));
        }

        private void AddPoint(Point point)
        {
            _points.Add(point);
        }

        private int PointsNumber()
        {
            return _points.Count;
        }
    }
}
