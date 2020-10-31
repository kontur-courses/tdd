using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace TagCloud
{
    public class Spiral
    {
        private Point center;
        private Direction lastDirection;
        private Point lastPoint;
        private int radius;

        public Spiral(Point center)
        {
            this.center = center;
            lastPoint = center;
            radius = 0;
            lastDirection = Direction.Up;
        }

        public Point GetNextPoint()
        {
            lastPoint = lastDirection switch
            {
                Direction.Up => new Point(lastPoint.X, lastPoint.Y - 1),
                Direction.Down => new Point(lastPoint.X, lastPoint.Y + 1),
                Direction.Left => new Point(lastPoint.X - 1, lastPoint.Y),
                _ => new Point(lastPoint.X + 1, lastPoint.Y)
            };

            if (Math.Abs(lastPoint.X - center.X) != radius || Math.Abs(lastPoint.Y - lastPoint.Y) != radius) return lastPoint;
            if (lastDirection == Direction.Up)
            {
                radius++;
                lastPoint.X--;
                lastPoint.Y--;
            }
            TurnDirection();

            return lastPoint;
        }

        private void TurnDirection()
        {
            lastDirection = lastDirection switch
            {
                Direction.Down => Direction.Left,
                Direction.Left => Direction.Up,
                Direction.Up => Direction.Right,
                _ => Direction.Down
            };
        }
    }
}