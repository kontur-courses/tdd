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
            
            
            if (previousRectangle != null)
            {
                if (sideToFill is SideToFill.Up)
                {
                    if ((nextRect.Right - filledArea.Right) / (double) nextRect.Width >= 0.5)
                    {
                        sideToFill = SideToFill.Right;
                        UpdateFilledRect();
                    }
                }

                else if (sideToFill is SideToFill.Right)
                {
                    if ((nextPoint.Y + rectangleSize.Height - filledArea.Bottom) / (double)rectangleSize.Height >= 0.5)
                    {
                        sideToFill = SideToFill.Down;
                        UpdateFilledRect();
                    }
                }
                else if (sideToFill is SideToFill.Down)
                {
                    if ((filledArea.Left - nextPoint.X) / (double)rectangleSize.Width >= 0.5)
                    {
                        sideToFill = SideToFill.Left;
                        UpdateFilledRect();
                    }
                }
                else if (sideToFill is SideToFill.Left)
                {
                    if ((filledArea.Top - nextRect.Top) / (double)rectangleSize.Height >= 0.5)
                    {
                        sideToFill = SideToFill.Up;
                        UpdateFilledRect();
                    }
                }
            }
            
            
            if (nextRect.Bottom > maxBottom)
                maxBottom = nextRect.Bottom;
            if (nextRect.Right > maxRight)
                maxRight = nextRect.Right;
            if (nextRect.Left < minLeft)
                minLeft = nextRect.Left;
            if (nextRect.Top < minTop)
                minTop = nextRect.Top;

            previousRectangle = nextRect;
            Rectangles.Add(nextRect);
            return nextRect;
        }

        private void UpdateFilledRect()
        {
            if (previousRectangle is null)
            {
                return;
            }

            filledArea = new Rectangle(minLeft, minTop, maxRight - minLeft, maxBottom - minTop);
        }

        private Point GetNextPoint(Size rectangleSize)
        {
            if (previousRectangle is null)
            {
                return new Point(filledArea.X - rectangleSize.Width, filledArea.Y);
            }
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
    }
}