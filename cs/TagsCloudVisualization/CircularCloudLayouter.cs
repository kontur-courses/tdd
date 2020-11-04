using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal enum Direction
    {
        Top,
        Right,
        Bottom,
        Left,
    }

    static class DirectionExtensions
    {
        public static Direction GetNextDirection(this Direction direction)
        {
            return direction switch
            {
                Direction.Bottom => Direction.Left,
                Direction.Left => Direction.Top,
                Direction.Top => Direction.Right,
                Direction.Right => Direction.Bottom,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public static int GetDirectionMultiplier(this Direction direction)
        {
            if (direction == Direction.Top || direction == Direction.Right)
                return 1;
            return -1;
        }
    }

    public class CircularCloudLayouter
    {
        private Direction direction = Direction.Top;
        private Direction nextDirection = Direction.Right;
        private Dictionary<Direction, int> extremeCoords;
        private readonly Dictionary<Direction, int> newExtremeCoords;
        public readonly List<Rectangle> Rectangles;
        public readonly int RectangleMargin;
        public readonly Point Center;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            RectangleMargin = 2;
            newExtremeCoords = new Dictionary<Direction, int>
            {
                {Direction.Top, center.Y},
                {Direction.Right, center.X},
                {Direction.Bottom, center.Y},
                {Direction.Left, center.X}
            };
        }
        
        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle {Size = rectangleSize};
            
            if (Rectangles.Count == 0)
            {
                rectangle.Location = Center;

                extremeCoords = new Dictionary<Direction, int>
                {
                    {Direction.Top, rectangle.Y + rectangle.Height + RectangleMargin},
                    {Direction.Right, rectangle.X + rectangle.Width + RectangleMargin},
                    {Direction.Bottom, rectangle.Y - RectangleMargin},
                    {Direction.Left, rectangle.X - RectangleMargin}
                };
                Rectangles.Add(rectangle);
                return rectangle;
            }
            
            rectangle.Location = CalculateLocation(rectangle, Rectangles[^1]);
            UpdateExtremeCoords(rectangle);
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private void UpdateExtremeCoords(Rectangle rectangle)
        {
            var sign = direction.GetDirectionMultiplier();
            var coord = GetNewExtremeCoordinate(rectangle, direction);
            newExtremeCoords[direction] = sign * Math.Max(sign * coord, sign * newExtremeCoords[direction]);
            
            coord = GetNewExtremeCoordinate(rectangle, nextDirection);
            sign = nextDirection.GetDirectionMultiplier();
            if (sign * (coord - extremeCoords[nextDirection]) > 0)
            {
                extremeCoords[direction] = newExtremeCoords[direction];
                extremeCoords[nextDirection] = coord;
                direction = nextDirection;
                nextDirection = direction.GetNextDirection();
            }
        }
        
        private int GetNewExtremeCoordinate(Rectangle rectangle, Direction d)
        {
            return d switch
            {
                Direction.Bottom => rectangle.Y - RectangleMargin,
                Direction.Left => rectangle.X - RectangleMargin,
                Direction.Top => rectangle.Y + rectangle.Height + RectangleMargin,
                Direction.Right => rectangle.X + rectangle.Width + RectangleMargin,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private Point CalculateLocation(Rectangle current, Rectangle previous)
        {
            return direction switch
            {
                Direction.Bottom => new Point(previous.X - current.Width - RectangleMargin,
                    extremeCoords[direction] - current.Height),
                Direction.Left => new Point(extremeCoords[direction] - current.Width,
                    previous.Y + previous.Height + RectangleMargin),
                Direction.Top => new Point(previous.X + previous.Width + RectangleMargin,
                    extremeCoords[direction]),
                Direction.Right => new Point(extremeCoords[direction],
                    previous.Y - current.Height - RectangleMargin),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}