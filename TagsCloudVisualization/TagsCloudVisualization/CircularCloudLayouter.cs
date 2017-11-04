using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace TagsCloudVisualization
{
    internal enum Side
    {
        Top, Left, Bottom, Right
    }

    public class CircularCloudLayouter
    {
        public readonly Point center;
        internal double radius;
        private readonly List<Rectangle> rectangles;
        private readonly Queue<(Side side, Rectangle rectangle)> positions;
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            rectangles = new List<Rectangle>();
            positions = new Queue<(Side side, Rectangle rectangle)>();
        }

        private Point GetLocation(Size wordBox)
        {
            var firstPosition = positions.Dequeue();
            var location = firstPosition.rectangle.Location;
            while (positions.Count != 0)
            {
                var position = positions.Dequeue();

                location = position.rectangle.Location;
                location = OffsetRectangleLocation(location, position.side, wordBox, position.rectangle.Size);

                var rectangle = new Rectangle(location, wordBox);
                var length = GetLengthToTheMostDistantPoint(center, rectangle);
                if (rectangles.Any(x => x.IntersectsWith(rectangle)))
                    continue;
                if (length <= radius)
                    break;
            }
            if (positions.Count == 0)
                location = OffsetRectangleLocation(location, firstPosition.side, wordBox, firstPosition.rectangle.Size);
            return location;
        }

        private static Point OffsetRectangleLocation(Point location, Side side, Size rectanleSize, Size oldRectangleSize)
        {
            switch (side)
            {
                case Side.Top:
                    location.Offset(0, -rectanleSize.Height);
                    break;
                case Side.Left:
                    location.Offset(-rectanleSize.Width, 0);
                    break;
                case Side.Bottom:
                    location.Offset(0, oldRectangleSize.Height);
                    break;
                case Side.Right:
                    location.Offset(oldRectangleSize.Width, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(side), side, null);
            }
            return location;
        }

        private static int GetLengthToTheMostDistantPoint(Point center, Rectangle rectangle)
        {
            var Dx = Math.Max(Math.Abs(center.X - rectangle.X),
                Math.Abs(center.X - rectangle.X - rectangle.Width));
            var Dy = Math.Max(Math.Abs(center.Y - rectangle.Y),
                Math.Abs(center.Y - rectangle.Y - rectangle.Height));
            var length = (int)Math.Sqrt(Math.Pow(Dx, 2) + Math.Pow(Dy, 2));
            return length;
        }

        private void AddNewPositions(Rectangle rectangle)
        {
            for (var i = 0; i < 4; i++)
                positions.Enqueue(((Side)i, rectangle));
        }
        public Rectangle PutNextRectangle(Size wordBox)
        {
            if (wordBox.Width < 0 || wordBox.Height < 0)
                throw new ArgumentException("Rectangle size must be a positive");
            var nextRectangle = new Rectangle
            {
                Size = wordBox,
                Location = rectangles.Count == 0
                    ? center - new Size(wordBox.Width / 2, wordBox.Height / 2)
                    : GetLocation(wordBox)
            };
            var length = GetLengthToTheMostDistantPoint(center, nextRectangle);
            if (length > radius)
                radius = length;
            rectangles.Add(nextRectangle);
            AddNewPositions(nextRectangle);
            return nextRectangle;
        }
    }
}
