using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualisation
{
    public partial class CircularCloudLayouter
    {
        public class CandidatePoint
        {
            public CandidatePoint(int x, int y, Point cloudCenter, PlacedRectangle parent,
                PointDirection direction)
            {
                X = x;
                Y = y;
                Parent = parent;
                Direction = direction;
                RelativeX = x - cloudCenter.X;
                RelativeY = y - cloudCenter.Y;
                CloudCenterDistance = Math.Sqrt(RelativeX * RelativeX + RelativeY * RelativeY);
            }

            public int X { get; }
            public int Y { get; }
            public int RelativeX { get; }
            public int RelativeY { get; }
            public double CloudCenterDistance { get; }
            public PlacedRectangle Parent { get; }
            public PointDirection Direction { get; }

            public static implicit operator Point(CandidatePoint point) => new Point(point.X, point.Y);
        }

        public class PlacedRectangle
        {
            public PlacedRectangle(Rectangle rectangle, Point center) : this(rectangle.Location, rectangle.Size, center)
            {
            }

            public PlacedRectangle(Point point, Size size, Point center) : this(point.X, point.Y, size.Width,
                size.Height, center)
            {
            }

            public PlacedRectangle(int x, int y, int width, int height, Point center)
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
                Right = Left + Width;
                Bottom = Y + Height;
                Location = new Point(X, Y);
                Size = new Size(Width, Height);

                Corners = new[]
                {
                    new CandidatePoint(Left, Top, center, this, PointDirection.Up),
                    new CandidatePoint(Right, Top, center, this, PointDirection.Right),
                    new CandidatePoint(Left, Bottom, center, this, PointDirection.Left),
                    new CandidatePoint(Right, Bottom, center, this, PointDirection.Down)
                };
            }

            public int X { get; }
            public int Y { get; }
            public int Width { get; }
            public int Height { get; }

            public int Top => Y;
            public int Bottom { get; }
            public int Left => X;
            public int Right { get; }

            public Point Location { get; }
            public Size Size { get; }

            public IEnumerable<CandidatePoint> Corners { get; }

            public bool Intersected(Rectangle r2) => Left <= r2.Right && r2.Left <= Right &&
                                                     Top <= r2.Bottom && r2.Top <= Bottom;

            public static implicit operator Rectangle(PlacedRectangle r) => new Rectangle(r.Location, r.Size);
        }

        public enum PointDirection
        {
            Up,
            Down,
            Left,
            Right
        }
    }
}