using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudVisualization
{
    using static CircularCloudLayouter;
    static class TagCloudVisualisation_Extensions
    {
        public static Directions Next(this Directions direction) =>
            (Directions) ((int) (direction + 1) % 4);

        public static Directions Previous(this Directions direction)
        {
            var result = (int) direction - 1;
            if (result == -1) result = 3;
            return (Directions) result;
        }

        public static Directions Opposite(this Directions direction) => direction.Next().Next();

        public static bool IsNormal(this Directions direction) =>
            direction == Directions.Right || direction == Directions.Down;

        public static Size GetOffset(this Directions direction, int offset = 1) =>
            direction switch
            {
                Directions.Up => new Size(0, -offset),
                Directions.Left => new Size(-offset, 0),
                Directions.Down => new Size(0, offset),
                Directions.Right => new Size(offset, 0),
                _ => new Size(0, 0)
            };
        
        public static Rectangle Displaced(this Rectangle rectangle, Size offset) =>
            new Rectangle(rectangle.Location + offset, rectangle.Size);

        public static Rectangle Displaced(this Rectangle rectangle, Point offset) => rectangle.Displaced(new Size(offset));

        public static int SizeInDirection(this Rectangle rectangle, CircularCloudLayouter.Directions direction) =>
            direction switch
            {
                Directions.Up => rectangle.Height,
                Directions.Left => rectangle.Width,
                Directions.Down => rectangle.Height,
                Directions.Right => rectangle.Width,
                _ => 0
            };
        
        public static int TopInDirection(this Rectangle rectangle, Directions direction) =>
            direction switch
            {
                Directions.Up => rectangle.Top,
                Directions.Left => rectangle.Left,
                Directions.Down => rectangle.Bottom,
                Directions.Right => rectangle.Right,
                _ => 0
            };
        
        public static Rectangle ResizedToTopInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            var result = new Rectangle(rectangle.Location, rectangle.Size);
            switch (direction)
            {
                case Directions.Up:
                    var offset = direction.GetOffset(value - result.Top);
                    result.Location -= offset;
                    result.Size += offset;
                    break;
                case Directions.Left:
                    offset = direction.GetOffset(value - result.Left);
                    result.Location -= offset;
                    result.Size += offset;
                    break;
                case Directions.Down: result.Size += direction.GetOffset(value - result.Bottom); break;
                case Directions.Right:result.Size += direction.GetOffset(value - result.Right); break;
            }

            return result;
        }
        
        public static Rectangle DisplacedToTopInDirection(this Rectangle rectangle, Directions direction, int value)
        {
            var result = new Rectangle(rectangle.Location, rectangle.Size);
            result.Location += direction.GetOffset((value - result.TopInDirection(direction))
                                                   * (direction.IsNormal() ? 1 : -1));
            return result;
        }

        public static bool IntersectInDirection(this Rectangle r1, Rectangle r2, Directions direction)
        {
            if (direction == Directions.Up || direction == Directions.Down)
            {
                r1.Location = new Point(0, r1.Location.Y);
                r2.Location = new Point(0, r2.Location.Y);
            }
            else
            {
                r1.Location = new Point(r1.Location.X, 0);
                r2.Location = new Point(r2.Location.X, 0);
            }

            return r1.IntersectsWith(r2);
        }
    }
    public class CircularCloudLayouter
    {
        Point center;

        private Spiral spiral;
        public class Spiral
        {
            private Directions currentDirection = Directions.Up;
            private Rectangle sizeRect, prevSizeRect;
            private List<Rectangle>[] lines = new List<Rectangle>[4];
            private List<Rectangle> currentLine;

            public Spiral(Size firstRectangleSize)
            {
                currentLine = new List<Rectangle> {new Rectangle(Point.Empty, firstRectangleSize)};
                sizeRect = new Rectangle(Point.Empty, firstRectangleSize);
            }

            private void Turn()
            {
                lines[(int) currentDirection] = currentLine;

                var lastRect = currentLine[currentLine.Count - 1];
                currentLine = new List<Rectangle> {lastRect};
                
                prevSizeRect = sizeRect;
                
                var lastRectTop = lastRect.TopInDirection(currentDirection);
                if (Math.Abs(lastRectTop) > Math.Abs(sizeRect.TopInDirection(currentDirection)))
                    sizeRect = sizeRect.ResizedToTopInDirection(currentDirection, lastRectTop);

                currentDirection = currentDirection.Next();
            }

            public Rectangle PutNextRectangle(Size size)
            {
                var lastRect = currentLine[currentLine.Count - 1];
                var previousDirection = currentDirection.Previous();
                var nextDirection = currentDirection.Next();
                var rect = new Rectangle(lastRect.Location, size);
                rect = rect
                    .Displaced(currentDirection.GetOffset(
                        (currentDirection.IsNormal() ? lastRect : rect).SizeInDirection(currentDirection)))
                    .DisplacedToTopInDirection(nextDirection,
                        lastRect.TopInDirection(currentDirection.Next()));

                var previousLine = lines[(int) currentDirection];
                var prevDirNormalMod = previousDirection.IsNormal() ? 1 : -1;
                var curDirNormalMod = currentDirection.IsNormal() ? 1 : -1;
                if (previousLine != null)
                {
                    var max = int.MinValue;
                    foreach (var intersectedRectangle in previousLine.Where(r => r.IntersectInDirection(rect, currentDirection)))
                    {
                        var curTop = intersectedRectangle.TopInDirection(previousDirection);
                        if (curTop * prevDirNormalMod > max * prevDirNormalMod) max = curTop;
                    }

                    rect = rect.DisplacedToTopInDirection(previousDirection.Opposite(), max);
                }
                currentLine.Add(rect);
                var rectTopInDirection = rect.TopInDirection(previousDirection);
                
                if (rectTopInDirection * prevDirNormalMod > sizeRect.TopInDirection(previousDirection) * prevDirNormalMod)
                    sizeRect = sizeRect.ResizedToTopInDirection(currentDirection, rectTopInDirection);
                else if(rect.TopInDirection(currentDirection) * curDirNormalMod > prevSizeRect.TopInDirection(currentDirection) * curDirNormalMod)
                    Turn();

                return rect;
            }
        }

        public enum Directions
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }
        
        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            if(size.Width <= 0 || size.Height <= 0) throw new ArgumentException();
            if (spiral == null)
            {
                spiral = new Spiral(size);
                return new Rectangle(center, size);
            }

            return spiral.PutNextRectangle(size).Displaced(center);
        }
    }

    [TestFixture]
    public class CircularCloudLayouter_Should
    {
        [Test]
        public void ThrowException_WhenPutNotPositiveSize()
        {
            var layouter = new CircularCloudLayouter(new Point(7, 8));

            Action act = () => layouter.PutNextRectangle(new Size(-1, 0));
            act.Should().Throw<ArgumentException>();
        }

        [Test]
        public void PlaceFirstFourRectanglesAroundCenter()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            
            var rect = layouter.PutNextRectangle(rectangles[0]);
            rect.Location.Should().Be(center);

            rect = layouter.PutNextRectangle(rectangles[1]);
            rect.X.Should().Be(center.X);
            rect.Bottom.Should().Be(center.Y);

            rect = layouter.PutNextRectangle(rectangles[2]);
            rect.Right.Should().Be(center.X);
            rect.Bottom.Should().Be(center.Y);
            
            rect = layouter.PutNextRectangle(rectangles[3]);
            rect.Right.Should().Be(center.X);
            rect.Y.Should().Be(center.Y);
        }

        [Test]
        public void NotHaveIntersections_WithFourRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result
                .Where(r => result.Where(rr => rr != r).Any(r.IntersectsWith)).Should().BeEmpty();
        }
        
        [Test]
        public void NotChangeSize_WithFourRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangles, options => options.WithStrictOrdering());
        }
        
        [Test]
        public void NotChangeSize_WithALotRectangles()
        {
            var center = new Point(7, 10);
            var layouter = new CircularCloudLayouter(center);

            var rectangles = new[]
            {
                new Size(7, 3),
                new Size(5, 8),
                new Size(3, 6),
                new Size(2, 1),
                new Size(7, 5),
                new Size(3, 10),
                new Size(15, 1),
                new Size(31, 10),
                new Size(40, 12),
            };
            var result = rectangles.Select(r => layouter.PutNextRectangle(r)).ToArray();
            result.Select(r => r.Size).Should().BeEquivalentTo(rectangles, options => options.WithStrictOrdering());
        }
    }
}