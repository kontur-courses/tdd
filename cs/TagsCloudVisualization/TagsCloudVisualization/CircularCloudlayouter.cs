using System;
using System.Collections.Generic;
using System.Drawing;


namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public Point Center { get; set; }
        private readonly FirstSector _firstSector;
        private readonly Direction _direction;
        public List<Rectangle> Rectangles;

        public CircularCloudLayouter(Point center)
        {
            if (center.X < 0 || center.Y < 0)
                throw new ArgumentException("both center coordinates should be non-negative");

            Center = center;
            _firstSector = new FirstSector();
            _direction = new Direction();
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {   
            // В текущий момент подразумеваем 0 < direction < Math.PI / 2
            var direction = _direction.GetNextDirection();

            var locationOfNewRectangle = FindAvailableLocationForRectangle(_firstSector, direction);
            RecalculateFirstSector(locationOfNewRectangle, rectangleSize);

            var result = new Rectangle(locationOfNewRectangle, rectangleSize);
            Rectangles.Add(result);

            return result;
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
            _firstSector.GetPoints().RemoveRange(rangeToRemove.Item1, rangeToRemove.Item2);
        }

        public void InsertNewPointsCreatedByRectangle(int index, int maxX, int maxY)
        {
            // Косяк. По факту от 1 до 3 новых точек. 3 в данном случае.
            _firstSector.GetPoints().Insert(index, new Point(maxX, maxY));
        }

        public Tuple<int, int> FindPointsRangeToRemove(int maxX, int maxY)
        {
            var rangeToRemove = new List<int>();
            var points = _firstSector.GetPoints();
            var point = new Point();
            for (int index = 0; index < points.Count; index++)
            {
                point = points[index];
                if (point.X <= maxX && point.Y <= maxY)
                    rangeToRemove.Add(index);
            }

            return new Tuple<int, int>(rangeToRemove[0], rangeToRemove[rangeToRemove.Count-1] + 1);
        }

        public Point FindAvailableLocationForRectangle(FirstSector firstSector, double direction)
        {
            if (firstSector.Count() == 1)
                return firstSector.GetPoints()[0];

            return FindFirstPointUnderDirectLine(firstSector.GetPoints(), direction);
        }

        public Point FindFirstPointUnderDirectLine(List<Point> points, double direction)
        {
            // y = x * tgAlpha. tgAlpha is direction.
            for (int index = 0; index < points.Count; index++)
            {
                var point = points[index];
                if (IsPointOnDirectLine(point.X, point.Y, direction))
                    return point;

                if (IsPointUnderDirectLine(point.X, point.Y, direction))
                    return points[index - 1].X == point.X ? point : points[index - 1];
            }

            return points[points.Count-1];
        }

        public bool IsPointUnderDirectLine(int x, int y, double direction)
        {
            return y < x * direction;
        }

        public bool IsPointOnDirectLine(int x, int y, double direction)
        {
            return y == x * direction;
        }

    }
}
