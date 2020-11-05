using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly Point center;

        private Spiral spiral;
        public class Spiral
        {
            private Directions currentDirection = Directions.Up;
            private Rectangle sizeRect, prevSizeRect;
            private readonly List<Rectangle>[] lines = new List<Rectangle>[4];
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
                
                var lastRectTop = lastRect.BorderInDirection(currentDirection);
                if (Math.Abs(lastRectTop) > Math.Abs(sizeRect.BorderInDirection(currentDirection)))
                    sizeRect = sizeRect.ResizedToBorderInDirection(currentDirection, lastRectTop);

                currentDirection = currentDirection.CounterClockwise();
            }

            public Rectangle PutNextRectangle(Size size)
            {
                var lastRect = currentLine[currentLine.Count - 1];
                var previousDirection = currentDirection.Clockwise();
                var nextDirection = currentDirection.CounterClockwise();
                var rect = new Rectangle(lastRect.Location, size);
                rect = rect
                    .Shifted(currentDirection.GetOffset(
                        (currentDirection.IsPositive() ? lastRect : rect).SizeInDirection(currentDirection)))
                    .ShiftedToBorderInDirection(nextDirection,
                        prevSizeRect.BorderInDirection(previousDirection));

                var previousLine = lines[(int) currentDirection];
                var prevDirNormalMod = previousDirection.IsPositive() ? 1 : -1;
                var curDirNormalMod = currentDirection.IsPositive() ? 1 : -1;
                if (previousLine != null)
                {
                    var max = int.MinValue;
                    foreach (var intersectedRectangle in previousLine.Where(r => r.IntersectInDirection(rect, currentDirection)))
                    {
                        var curTop = intersectedRectangle.BorderInDirection(previousDirection);
                        if (curTop * prevDirNormalMod > max * prevDirNormalMod) max = curTop;
                    }

                    rect = rect.ShiftedToBorderInDirection(previousDirection.Opposite(), max);
                }
                currentLine.Add(rect);
                var rectTopInDirection = rect.BorderInDirection(previousDirection);
                
                if(rect.BorderInDirection(currentDirection) * curDirNormalMod > prevSizeRect.BorderInDirection(currentDirection) * curDirNormalMod)
                    Turn();
                if (rectTopInDirection * prevDirNormalMod > sizeRect.BorderInDirection(previousDirection) * prevDirNormalMod)
                    sizeRect = sizeRect.ResizedToBorderInDirection(currentDirection, rectTopInDirection);

                return rect;
            }
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

            return spiral.PutNextRectangle(size).Shifted(center);
        }
    }
}