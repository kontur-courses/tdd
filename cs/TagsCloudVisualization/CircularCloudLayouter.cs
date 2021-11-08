using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    internal class CircularCloudLayouter
    {
        public Rectangle filledArea;
        public List<Rectangle> Rectangles = new List<Rectangle>();
        private Rectangle? previousRectangle = null;
        private SideToFill sideToFill = SideToFill.Left;

        private int maxBottom;
        private int minTop;
        private int minLeft;
        private int maxRight;

        public CircularCloudLayouter(Point center)
        {
            filledArea = new Rectangle(center, Size.Empty);
            maxBottom = center.Y;
            minTop = center.Y;
            maxRight = center.X;
            minLeft = center.X;
        }

        public Rectangle PullNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
            {
                throw new ArgumentException(
                    $"Size is empty! Width: {rectangleSize.Width}, height: {rectangleSize.Height}");
            }
            var nextPoint = GetNextPoint(rectangleSize);
            var nextRect = new Rectangle(nextPoint, rectangleSize);
            nextRect = Move(nextRect);
            
            if (previousRectangle != null)
                CheckAndSwitchSideToFill(rectangleSize, nextRect, nextPoint);
            
            UpdateSizeFilledRect(nextRect);

            previousRectangle = nextRect;
            Rectangles.Add(nextRect);
            return nextRect;
        }

        private void UpdateSizeFilledRect(Rectangle nextRect)
        {
            if (nextRect.Bottom > maxBottom)
                maxBottom = nextRect.Bottom;
            
            if (nextRect.Right > maxRight)
                maxRight = nextRect.Right;
            
            if (nextRect.Left < minLeft)
                minLeft = nextRect.Left;
            
            if (nextRect.Top < minTop)
                minTop = nextRect.Top;
        }

        private void CheckAndSwitchSideToFill(Size rectangleSize, Rectangle nextRect, Point nextPoint)
        {
            if (sideToFill == SideToFill.Up && (nextRect.Right - filledArea.Right) / (double)nextRect.Width >= 0.5)
            {
                sideToFill = SideToFill.Right;
                UpdateFilledRect();
            }
            else if (sideToFill == SideToFill.Right &&
                     (nextPoint.Y + rectangleSize.Height - filledArea.Bottom) / (double)rectangleSize.Height >= 0.5)
            {
                sideToFill = SideToFill.Down;
                UpdateFilledRect();
            }
            else if (sideToFill == SideToFill.Down &&
                     (filledArea.Left - nextPoint.X) / (double)rectangleSize.Width >= 0.5)
            {
                sideToFill = SideToFill.Left;
                UpdateFilledRect();
            }
            else if (sideToFill == SideToFill.Left &&
                     (filledArea.Top - nextRect.Top) / (double)rectangleSize.Height >= 0.5)
            {
                sideToFill = SideToFill.Up;
                UpdateFilledRect();
            }
        }

        private void UpdateFilledRect()
        {
            if (previousRectangle is null)
                return;

            filledArea = new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            if (previousRectangle is null)
                return new Point(filledArea.X - rectangleSize.Width, filledArea.Y);

            var newPoint = new Point();
            
            if (sideToFill is SideToFill.Up)
            {
                newPoint.X = previousRectangle.Value.Right;
                newPoint.Y = filledArea.Top - rectangleSize.Height;
            }
            else if (sideToFill is SideToFill.Right)
            {
                newPoint.X = filledArea.Right;
                newPoint.Y = previousRectangle.Value.Bottom;
            }
            else if (sideToFill is SideToFill.Down)
            {
                newPoint.X = previousRectangle.Value.X - rectangleSize.Width;
                newPoint.Y = filledArea.Bottom;
            }
            else if (sideToFill is SideToFill.Left)
            {
                newPoint.X = filledArea.X - rectangleSize.Width;
                newPoint.Y = previousRectangle.Value.Y - rectangleSize.Height;
            }

            return newPoint;
        }

        private Rectangle Move(Rectangle nextRectangle)
        {
            var maxPossibleDistanceRight = int.MaxValue;
            var maxPossibleDistanceLeft = int.MaxValue;
            var maxPossibleDistanceUp = int.MaxValue;
            var maxPossibleDistanceDown = int.MaxValue;
            
            foreach (var rectangle in Rectangles)
            {
                if (HaveIntersectionOnX(rectangle, nextRectangle))
                {
                    if (rectangle.Right <= nextRectangle.Left)
                    {
                        maxPossibleDistanceLeft =
                            Math.Min(nextRectangle.Left - rectangle.Right, maxPossibleDistanceLeft);
                    }
                    else if (rectangle.Left >= nextRectangle.Right)
                    {
                        maxPossibleDistanceRight =
                            Math.Min(rectangle.Left - nextRectangle.Right, maxPossibleDistanceRight);
                    }
                }
            
                if (HaveIntersectionOnY(rectangle, nextRectangle))
                {
                    if (rectangle.Bottom <= nextRectangle.Top)
                    {
                        maxPossibleDistanceUp = Math.Min(nextRectangle.Top - rectangle.Bottom, maxPossibleDistanceUp);
                    }
                    else if (rectangle.Top >= nextRectangle.Bottom)
                    {
                        maxPossibleDistanceDown =
                            Math.Min(rectangle.Top - nextRectangle.Bottom, maxPossibleDistanceDown);
                    }
                }
            }
            
            if (maxPossibleDistanceDown > 0 && maxPossibleDistanceDown != int.MaxValue && sideToFill is SideToFill.Up)
                nextRectangle.Y += maxPossibleDistanceDown;
            if (maxPossibleDistanceLeft > 0 && maxPossibleDistanceLeft != int.MaxValue && sideToFill is SideToFill.Right)
                nextRectangle.X -= maxPossibleDistanceLeft;
            if (maxPossibleDistanceRight > 0 && maxPossibleDistanceRight != int.MaxValue && sideToFill is SideToFill.Left)
                nextRectangle.X += maxPossibleDistanceRight;
            if (maxPossibleDistanceUp > 0 && maxPossibleDistanceUp != int.MaxValue && sideToFill is SideToFill.Down)
                nextRectangle.Y -= maxPossibleDistanceUp;

            return nextRectangle;
        }

        private bool HaveIntersectionOnX(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Top > rect2.Top && rect1.Top < rect2.Bottom
                   || rect1.Bottom > rect2.Top && rect1.Bottom < rect2.Bottom
                   || rect1.Top >= rect2.Top && rect1.Bottom <= rect2.Bottom
                   || rect1.Top <= rect2.Top && rect1.Bottom >= rect2.Bottom;
        }

        private bool HaveIntersectionOnY(Rectangle rect1, Rectangle rect2)
        {
            return rect1.Right > rect2.Left && rect1.Right < rect2.Right
                   || rect1.Left > rect2.Left && rect1.Left < rect2.Right
                   || rect1.Left <= rect2.Left && rect1.Right >= rect2.Right
                   || rect1.Left >= rect2.Left && rect1.Right <= rect2.Right;
        }
    }
}