using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class FirstSector
    {
        private readonly List<Point> _points;

        public FirstSector()
        {
            _points = new List<Point>();
            AddPoint(0, 0);
        }

        public Rectangle PlaceNewRectangle(double direction, Size rectangleSize)
        {
            var locationOfNewRectangle = FindAvailableLocationForRectangle(direction);
            RecalculateFirstSector(locationOfNewRectangle, rectangleSize);

            return new Rectangle(locationOfNewRectangle.X, locationOfNewRectangle.Y, rectangleSize.Width, rectangleSize.Height);
        }

        public Point FindAvailableLocationForRectangle(double direction)
        {
            // y = x * tgAlpha. tgAlpha is direction.
            for (int index = 0; index < Count(); index++)
            {
                var point = _points[index];
                if (IsPointOnDirectLine(point.X, point.Y, direction))
                    return point;

                if (IsPointUnderDirectLine(point.X, point.Y, direction))
                    return _points[index - 1].X == point.X ? point : _points[index - 1];
            }

            return _points[Count() - 1];
        }

        public void RecalculateFirstSector(Point locationOfNewRectangle, Size rectangleSize)
        {
            var maxX = locationOfNewRectangle.X + rectangleSize.Width;
            var maxY = locationOfNewRectangle.Y + rectangleSize.Height;

            var rangeToRemove = FindPointsRangeToRemove(maxX, maxY);

            RemovePointsUnderNewRectangle(rangeToRemove);
            InsertNewPointsCreatedByRectangle(rangeToRemove.Item1, maxX, maxY);
        }

        public void RemovePointsUnderNewRectangle(Tuple<int, int> rangeToRemove)
        {
            _points.RemoveRange(rangeToRemove.Item1, rangeToRemove.Item2);
        }

        public void InsertNewPointsCreatedByRectangle(int index, int maxX, int maxY)
        {
            // Косяк. По факту от 1 до 3 новых точек. 3 в данном случае.
            _points.Insert(index, new Point(maxX, maxY));
        }

        public Tuple<int, int> FindPointsRangeToRemove(int maxX, int maxY)
        {
            var rangeToRemove = new List<int>();

            for (int index = 0; index < Count(); index++)
            {
                var point = _points[index];
                if (point.X <= maxX && point.Y <= maxY)
                    rangeToRemove.Add(index);
            }

            return new Tuple<int, int>(rangeToRemove[0], rangeToRemove[rangeToRemove.Count - 1] + 1);
        }


        public bool IsPointUnderDirectLine(int x, int y, double direction)
        {
            return y < x * direction;
        }

        public bool IsPointOnDirectLine(int x, int y, double direction)
        {
            return y == x * direction;
        }

        public void AddPoint(int x, int y)
        {
            _points.Add(new Point(x, y));
        }

        public int Count()
        {
            return _points.Count;
        }
    }
}
