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
        Center
    }
    
    public class CircularCloudLayouter
    {
        public Point Center { get; }
        public readonly int RectangleMargin;
        public readonly List<Rectangle> Rectangles;
        private Direction direction = Direction.Center;
        private Dictionary<Direction, int> extremeCoords;
        private readonly Dictionary<Direction, int> newExtremeCoords;

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
            RectangleMargin = 2;
            newExtremeCoords = new Dictionary<Direction, int>
            {
                {Direction.Top, 0},
                {Direction.Right, 0},
                {Direction.Bottom, 0},
                {Direction.Left, 0}
            };
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle {Size = rectangleSize};

            switch (direction)
            {
                case Direction.Top:
                    if (Rectangles[^1].Location.Equals(Center))
                    {
                        rectangle.Location = new Point(
                            Rectangles[^1].Location.X,
                            extremeCoords[direction]); //+ rectangle.Height
                    }
                    else
                    {
                        rectangle.Location = new Point(
                            Rectangles[^1].Location.X + Rectangles[^1].Width + RectangleMargin,
                            extremeCoords[direction]
                        );
                    }

                    newExtremeCoords[direction] = Math.Max(
                        rectangle.Location.Y + rectangle.Height + RectangleMargin,
                        newExtremeCoords[direction]);

                    if (rectangle.Location.X + rectangle.Width + RectangleMargin > extremeCoords[Direction.Right])
                    {
                        extremeCoords[Direction.Right] = Math.Max(
                            rectangle.Location.X + rectangle.Width + RectangleMargin,
                            extremeCoords[Direction.Right]
                        );
                        extremeCoords[direction] = newExtremeCoords[direction];
                        direction = Direction.Right;
                    }
                    break;
                case Direction.Bottom:
                    rectangle.Location = new Point(
                        Rectangles[^1].Location.X - rectangle.Width - RectangleMargin,
                        extremeCoords[direction] - rectangle.Height);
                    
                    newExtremeCoords[direction] = Math.Min(
                        rectangle.Location.Y - rectangle.Height - RectangleMargin,
                        newExtremeCoords[direction]);
                    
                    if (rectangle.Location.X - RectangleMargin < extremeCoords[Direction.Left])
                    {
                        extremeCoords[Direction.Left] = Math.Min(
                            rectangle.Location.X - RectangleMargin,
                            extremeCoords[Direction.Left]);
                        extremeCoords[direction] = newExtremeCoords[direction];
                        direction = Direction.Left;
                    }
                    break;
                case Direction.Left:
                    rectangle.Location = new Point(
                        extremeCoords[direction] - rectangle.Width,
                        Rectangles[^1].Location.Y + Rectangles[^1].Height + RectangleMargin);
                    
                    newExtremeCoords[direction] = Math.Min(
                        rectangle.Location.X - RectangleMargin,
                        newExtremeCoords[direction]);
                    
                    if (rectangle.Location.Y + rectangle.Height + RectangleMargin > extremeCoords[Direction.Top])
                    {
                        extremeCoords[Direction.Top] = Math.Max(
                            rectangle.Location.Y + rectangle.Height + RectangleMargin,
                            extremeCoords[Direction.Top]);
                        extremeCoords[direction] = newExtremeCoords[direction];
                        direction = Direction.Top;
                    }
                    break;
                case Direction.Right:
                    rectangle.Location = new Point(
                        extremeCoords[direction],
                        Rectangles[^1].Location.Y - rectangle.Height - RectangleMargin
                    );
                    
                    newExtremeCoords[direction] = Math.Max(
                        rectangle.Location.X + rectangle.Width + RectangleMargin,
                        newExtremeCoords[direction]);
                    
                    if (rectangle.Location.Y - RectangleMargin < extremeCoords[Direction.Bottom])
                    {
                        extremeCoords[Direction.Bottom] = Math.Min(
                            rectangle.Location.Y - RectangleMargin,
                            extremeCoords[Direction.Bottom]);
                        extremeCoords[direction] = newExtremeCoords[direction];
                        direction = Direction.Bottom;
                    }
                    break;
                case Direction.Center:
                    direction = Direction.Top;
                    rectangle.Location = Center;
                    extremeCoords = new Dictionary<Direction, int>
                    {
                        {Direction.Top, rectangle.Height + RectangleMargin},
                        {Direction.Right, rectangleSize.Width + RectangleMargin},
                        {Direction.Bottom, -RectangleMargin},
                        {Direction.Left, -RectangleMargin}
                    };
                    break;
            }
            Rectangles.Add(rectangle);
            return rectangle;
        }

        private Direction GetNextDirection(Direction direction)
        {
            var newDirection = ((int) direction + 1) % 4;
            return (Direction)newDirection;
        }
    }
}