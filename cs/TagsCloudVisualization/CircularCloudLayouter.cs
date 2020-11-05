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

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size size)
        {
            if (size.Width <= 0 || size.Height <= 0)
                throw new ArgumentException("Size should have positive arguments");
            
            if (spiral == null)
            {
                spiral = new Spiral(size);
                return new Rectangle(center, size);
            }

            return spiral.PutNextRectangle(size).Shifted(center);
        }

        private class Spiral
        {
            private Directions currentDirection = Directions.Up;
            private Rectangle sizeRectangle;
            private Rectangle previousSizeRectangle;
            
            private readonly List<Rectangle>[] lines = new List<Rectangle>[4];
            private List<Rectangle> currentLine;

            public Spiral(Size firstRectangleSize)
            {
                currentLine = new List<Rectangle> {new Rectangle(Point.Empty, firstRectangleSize)};
                sizeRectangle = new Rectangle(Point.Empty, firstRectangleSize);
            }

            private void Turn()
            {
                lines[(int) currentDirection] = currentLine;

                var lastRectangle = currentLine[currentLine.Count - 1];
                currentLine = new List<Rectangle> {lastRectangle};
                
                previousSizeRectangle = sizeRectangle;
                
                var lastRectangleBorder = lastRectangle.BorderInDirection(currentDirection);
                var sizeRectangleBorder = sizeRectangle.BorderInDirection(currentDirection);
                if (currentDirection.CompareInDirection(lastRectangleBorder, sizeRectangleBorder) > 0)
                    sizeRectangle = sizeRectangle.ResizedToBorderInDirection(currentDirection, lastRectangleBorder);

                currentDirection = currentDirection.CounterClockwise();
            }

            public Rectangle PutNextRectangle(Size size)
            {
                var toCenterDirection = currentDirection.CounterClockwise();
                var fromCenterDirection = currentDirection.Clockwise();
                var lastRectangle = currentLine[currentLine.Count - 1];
                
                var currentRectangle = new Rectangle(lastRectangle.Location, size);

                currentRectangle = currentRectangle
                    .ShiftedToRectangleBorder(currentDirection, lastRectangle)
                    .ShiftedToRectangleBorder(fromCenterDirection, previousSizeRectangle);

                var previousLine = lines[(int) currentDirection];
                if (previousLine != null)
                {
                    var intersectedRectangles = previousLine
                        .Where(r => r.IntersectInDirection(currentRectangle, currentDirection)).ToArray();
                    var max = intersectedRectangles.Length > 0
                        ? intersectedRectangles.MaxBorderInDirection(fromCenterDirection)
                        : previousSizeRectangle.BorderInDirection(fromCenterDirection);

                    currentRectangle = currentRectangle.ShiftedToBorderInDirection(toCenterDirection, max);
                }
                currentLine.Add(currentRectangle);
                
                var currentRectangleBorder = currentRectangle.BorderInDirection(fromCenterDirection);
                var sizeRectangleBorder = sizeRectangle.BorderInDirection(fromCenterDirection);
                if(fromCenterDirection.CompareInDirection(currentRectangleBorder, sizeRectangleBorder) > 0)
                    sizeRectangle = sizeRectangle.ResizedToBorderInDirection(fromCenterDirection, currentRectangleBorder);

                if (currentDirection.CompareInDirection(
                    currentRectangle.BorderInDirection(currentDirection),
                    previousSizeRectangle.BorderInDirection(currentDirection)) > 0)
                    Turn();

                return currentRectangle;
            }
        }
    }
}