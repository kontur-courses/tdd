using System;

namespace TagsCloudVisualization
{
    public struct Point
    {
        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
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

        public static Point operator +(Point left, Point right) => new Point(left.X + right.X, left.Y + right.Y);

        public static Point operator *(double left, Point right)
        {
            if (left < double.Epsilon)
                return Zero;
            var x = left * right.X;
            var y = left * right.Y;
            var signX = Math.Sign(x);
            var signY = Math.Sign(y);
            //Данная конструкция для избавления от неверного поведения координат при умножении на малые числа
            return new Point((int) (signX * Math.Ceiling(Math.Abs(x))), (int) (signY * Math.Ceiling(Math.Abs(y))));
        }

        public static Point Zero => new Point();

        public static Point operator *(Point left, double right) => right * left;
        public static Point operator -(Point left, Point right) => new Point(left.X - right.X, left.Y - right.Y);
        public static Point operator -(Point point) => new Point(-point.X, -point.Y);

        public override string ToString() => $"Point {{X={X}, Y={Y}}}";
        public int X { get; }
        public int Y { get; }

        public static implicit operator System.Drawing.Point(Point point) => new System.Drawing.Point(point.X, point.Y);

        public static implicit operator Point(System.Drawing.Point point) => new Point(point.X, point.Y);
    }
}
