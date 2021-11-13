using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        private readonly List<Rectangle> rectangles = new List<Rectangle>();
        private Point center;
        private int angle;
        private readonly int step;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            step = 10;
            angle = 0;
        }

        public Rectangle[] GetPutRectangles()
        {
            return rectangles.ToArray();
        }

        public Point GetCenter()
        {
            return center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException(
                    $"Size is empty! Width: {rectangleSize.Width}, height: {rectangleSize.Height}");
            }
            
            Rectangle nextRectangle;

            do
            {
                var nextPoint = GetNextPoint();
                nextRectangle = new Rectangle(nextPoint, rectangleSize);

            } while (DoesIntersectPreviousRectangles(nextRectangle));

            Movement possibleMovementToCenter;

            do
            {
                possibleMovementToCenter = GetPossibleMovement(nextRectangle);
                var previousQuarter = GetQuarter(nextRectangle);
                nextRectangle = possibleMovementToCenter.MoveRectangle(nextRectangle);
                
                if (QuarterChanged(previousQuarter, nextRectangle))
                    break;
            } while (possibleMovementToCenter.CanDoMovement());

            rectangles.Add(nextRectangle);
            return nextRectangle;
        }

        private bool QuarterChanged(Quarter quarter, Rectangle nextRectangle)
        {
            return quarter != GetQuarter(nextRectangle);
        }

        private Point GetNextPoint()
        {
            var length = step / (2 * Math.PI) * angle * Math.PI / 180;
            var x = (int)(length * Math.Cos(angle)) + center.X;
            var y = (int)(length * Math.Sin(angle)) + center.Y;
            angle++;
            
            return new Point(x, y);
        }

        private bool DoesIntersectPreviousRectangles(Rectangle rectangle)
        {
            return rectangles.Any(x => x.IntersectsWith(rectangle));
        }

        private static bool CanMadeMoveOnDistance(int distance)
        {
            return distance > 0 && distance != int.MaxValue;
        }
        
        private Movement GetPossibleMovement(Rectangle rectangle)
        {
            var quarter = GetQuarter(rectangle);
            
            var maxPossibleDistanceUp = int.MaxValue;
            var maxPossibleDistanceDown = int.MaxValue;
            var maxPossibleDistanceRight = int.MaxValue;
            var maxPossibleDistanceLeft = int.MaxValue;
            
            foreach (var rect in rectangles)
            {
                if (DoesSegmentsIntersect(rect.Left, rect.Right, 
                    rectangle.Left, rectangle.Right))
                {
                    if (rect.Bottom <= rectangle.Top && (quarter is Quarter.I || quarter is Quarter.II))
                    {
                        maxPossibleDistanceUp = Math.Min(rectangle.Top - rect.Bottom, maxPossibleDistanceUp);
                    }
                    else if (rect.Top >= rectangle.Bottom && (quarter is Quarter.IV || quarter is Quarter.III))
                    {
                        maxPossibleDistanceDown = Math.Min(rect.Top - rectangle.Bottom, maxPossibleDistanceDown);
                    }
                }
                
                if (DoesSegmentsIntersect(rect.Top, rect.Bottom, 
                    rectangle.Top, rectangle.Bottom))
                {
                    if (rect.Right <= rectangle.Left && (quarter is Quarter.I || quarter is Quarter.IV))
                    {
                        maxPossibleDistanceLeft = Math.Min(rectangle.Left - rect.Right, maxPossibleDistanceLeft);
                    }
                    else if (rect.Left >= rectangle.Right && (quarter is Quarter.II || quarter is Quarter.III))
                    {
                        maxPossibleDistanceRight = Math.Min(rect.Left - rectangle.Right, maxPossibleDistanceRight);
                    }
                }
            }

            var result = new Movement(MovementType.Up, 0);
            
            if (CanMadeMoveOnDistance(maxPossibleDistanceDown))
            {
                result = new Movement(MovementType.Down, maxPossibleDistanceDown);
            }

            else if (CanMadeMoveOnDistance(maxPossibleDistanceUp))
            {
                result = new Movement(MovementType.Up, maxPossibleDistanceUp);
            }
            
            else if (CanMadeMoveOnDistance(maxPossibleDistanceLeft))
            {
                result = new Movement(MovementType.Left, maxPossibleDistanceLeft);
            }

            else if (CanMadeMoveOnDistance(maxPossibleDistanceRight))
            {
                result = new Movement(MovementType.Right, maxPossibleDistanceRight);
            }

            return result;
        }

        private Quarter GetQuarter(Rectangle rectangle)
        {
            if (rectangle.X < center.X)
            {
                return rectangle.Y < center.Y
                    ? Quarter.III 
                    : Quarter.II;
            }

            return rectangle.Y < center.Y 
                ? Quarter.IV 
                : Quarter.I;
        }

        private static bool DoesSegmentsIntersect(int firstSegmentStart, int firstSegmentEnd, int secondSegmentStart,
            int secondSegmentEnd)
        {
            return Math.Max(firstSegmentStart, secondSegmentStart) < Math.Min(firstSegmentEnd, secondSegmentEnd);
        }
    }
}