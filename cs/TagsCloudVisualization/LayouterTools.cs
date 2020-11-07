using System;
using System.Drawing;

namespace TagsCloudVisualization
{
    public static class LayouterTools
    {
        public static Point GetOffset(Size rectangleSize, Rectangle previous, Direction direction)
        {
            return direction switch
            {
                Direction.Top => new Point(0, previous.Height),
                Direction.Right => new Point(previous.Width, previous.Height - rectangleSize.Height),
                Direction.Bottom => new Point(previous.Width - rectangleSize.Width, -rectangleSize.Height),
                Direction.Left => new Point(-rectangleSize.Width, 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static double CalculateDistance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point2.X - point1.X, 2) + Math.Pow(point2.Y - point1.Y, 2));
        }
    }
}