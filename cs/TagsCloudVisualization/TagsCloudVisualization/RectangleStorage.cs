using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public enum Sector
    {
        First,
        Second,
        Third,
        Fourth
    }
    public class RectangleStorage
    {
        private readonly List<Point> _points;
        private readonly Point _center;

        public RectangleStorage(Point center)
        {
            _points = new List<Point>();
            _center = center;
            AddPoint(center.X, center.Y);
        }

        public Rectangle PlaceNewRectangle(double direction, Size rectangleSize)
        {
            var locationForNewRectangle = FindLocation(direction);
            RecalculateFirstSector(locationForNewRectangle, rectangleSize);

            return new Rectangle(locationForNewRectangle.X, locationForNewRectangle.Y, rectangleSize.Width, rectangleSize.Height);
        }

        public Point FindLocation(double direction)
        {
            // y = x * tgAlpha. tgAlpha is direction.
            var sector = DetermineSectorByDirection(direction);

            return FindAvailablePoint(sector, direction);
        }

        private Point FindAvailablePoint(Sector sector, double direction)
        {
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

        public static Sector DetermineSectorByDirection(double direction)
        {
            direction = Tools.AngleToStandardValue(direction);
            if (direction >= 0 && direction <= Math.PI / 2)
                return Sector.First;
            if (direction > Math.PI / 2 && direction <= Math.PI)
                return Sector.Second;
            if (direction > Math.PI && direction <= 3 * Math.PI / 2)
                return Sector.Third;

            return Sector.Fourth;
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
