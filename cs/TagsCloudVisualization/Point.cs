using System;

namespace TagsCloudVisualization
{
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point point)
                return X == point.X && Y == point.Y;
            return false;
        }

        /// <summary>
        ///     Point {X = 0, Y = 1}
        /// </summary>
        public static Point UnaryY => new Point(0, 1);

        /// <summary>
        ///     Point {X = 1, Y = 0}
        /// </summary>
        public static Point UnaryX => new Point(1, 0);

        public override int GetHashCode() => X ^ Y;

        public static bool operator ==(Point left, Point right) => Equals(left, right);

        public static bool operator !=(Point left, Point right) => !Equals(left, right);
        public static Point operator +(Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator *(double left, Point right) =>
            new Point((int) (left * right.X), (int) (left * right.Y));

        public static Point operator *(Point left, double right) => right * left;
        public static Point operator -(Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);
        public static Point operator -(Point point) => new Point(-point.X, -point.Y);

        public double Length => Math.Sqrt(X * X + Y * Y);

        public override string ToString() => $"Point {{X={X}, Y={Y}}}";
        public int X { get; }
        public int Y { get; }

        public static implicit operator System.Drawing.Point(Point point) => new System.Drawing.Point(point.X, point.Y);

        public static implicit operator Point(System.Drawing.Point point) => new Point(point.X, point.Y);
    }
}
