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
                        prevSizeRect.TopInDirection(previousDirection));

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
                
                if(rect.TopInDirection(currentDirection) * curDirNormalMod > prevSizeRect.TopInDirection(currentDirection) * curDirNormalMod)
                    Turn();
                if (rectTopInDirection * prevDirNormalMod > sizeRect.TopInDirection(previousDirection) * prevDirNormalMod)
                    sizeRect = sizeRect.ResizedToTopInDirection(currentDirection, rectTopInDirection);

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

            return spiral.PutNextRectangle(size).Displaced(center);
        }
    }
}