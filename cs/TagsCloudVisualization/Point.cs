using System;

namespace TagsCloudVisualization
{
    public class Point
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            var x = p1.X + p2.X;
            var y = p1.Y + p2.Y;

            return new Point(x, y);
        }

        public static Point operator /(Point p1, double value)
        {
            var x = p1.X / value;
            var y = p1.Y / value;

            return new Point(x, y);
        }

        public Point Normalized()
        {
            var x = 0;
            var y = 0;

            if (Math.Abs(X) > 1e-7)
                x = X < 0 ? -1 : 1;
            if (Math.Abs(Y) > 1e-7)
                y = Y < 0 ? -1 : 1;

            return new Point(x, y);
        }

        public override string ToString()
        {
            return $"Point (X: {X}; Y: {Y})";
        }

        public override bool Equals(object obj)
        {
            var otherPoint = (Point) obj;
            if (otherPoint == null)
                return false;
            return Math.Abs(otherPoint.X - X) < 1e-7 && Math.Abs(otherPoint.Y - Y) < 1e-7;
        }

        public bool EqualsApproximately(Point otherPoint)
        {
            var equalityValue = 1e-7;
            var deltaX = Math.Abs(X - otherPoint.X);
            var deltaY = Math.Abs(Y - otherPoint.Y);

            return deltaX < equalityValue && deltaY < equalityValue;
        }
    }
}