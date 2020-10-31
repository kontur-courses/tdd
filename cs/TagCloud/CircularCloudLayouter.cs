using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        public readonly List<Rectangle> Rectangles = new List<Rectangle>();
        private Point center;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var newRect = Rectangles.Count switch
            {
                0 => new Rectangle(new Point(center.X - rectangleSize.Width / 2, center.Y - rectangleSize.Height / 2),
                    rectangleSize),
                1 => new Rectangle(new Point(Rectangles[0].Location.X, Rectangles[0].Location.Y - rectangleSize.Height),
                    rectangleSize),
                _ => GetNewRectangle(rectangleSize)
            };
            if (newRect.Bottom < 0 || newRect.Height < 0 || newRect.Left < 0 || newRect.Right < 0)
                throw new Exception("Out of space");
            Rectangles.Add(newRect);
            return newRect;
        }

        private Rectangle GetNewRectangle(Size rectangleSize)
        {
            var lastRect = Rectangles[^1];
            var preLastRect = Rectangles[^2];
            var resultRect = Rectangle.Empty;
            if (lastRect.Bottom == preLastRect.Top)
                resultRect = Rectangles.Any(rect => rect != lastRect && lastRect.Bottom > rect.Top)
                    ? PutRectangleAbove(rectangleSize, lastRect)
                    : PutRectangleToTheRight(rectangleSize, lastRect);
            else if (lastRect.Top == preLastRect.Bottom)
                resultRect = Rectangles.Any(rect => rect != lastRect && lastRect.Top < rect.Bottom)
                    ? PutRectangleBelow(rectangleSize, lastRect)
                    : PutRectangleToTheLeft(rectangleSize, lastRect);
            else if (lastRect.Right == preLastRect.Left)
                resultRect = Rectangles.Any(rect => rect != lastRect && rect.Left < lastRect.Right)
                    ? PutRectangleToTheLeft(rectangleSize, lastRect)
                    : PutRectangleAbove(rectangleSize, lastRect);
            else
                resultRect = Rectangles.Any(rect => rect != lastRect && rect.Right > lastRect.Left)
                    ? PutRectangleToTheRight(rectangleSize, lastRect)
                    : PutRectangleBelow(rectangleSize, lastRect);

            return resultRect;
        }

        private Rectangle PutRectangleToTheLeft(Size rectangleSize, Rectangle lastRect)
        {
            var leftUpperCorner = lastRect.Location;
            var newRectLocation = new Point(leftUpperCorner.X - rectangleSize.Width, leftUpperCorner.Y);
            var borderRect = Rectangles.Where(rect =>
                    rect.Left < newRectLocation.X + rectangleSize.Width && newRectLocation.X < rect.Right)
                .OrderByDescending(rect => rect.Bottom)
                .FirstOrDefault();
            if (!borderRect.IsEmpty)
                newRectLocation.Y = borderRect.Bottom;
            var resultRect = new Rectangle(newRectLocation, rectangleSize);
            return resultRect;
        }

        private Rectangle PutRectangleBelow(Size rectangleSize, Rectangle lastRect)
        {
            var leftDownCorner = new Point(lastRect.Left, lastRect.Bottom);
            var borderRect = Rectangles.Where(rect =>
                        rect.Y < leftDownCorner.Y + rectangleSize.Height && leftDownCorner.Y < rect.Bottom)
                .OrderByDescending(rect => rect.Right)
                .FirstOrDefault();
            if (!borderRect.IsEmpty)
                leftDownCorner.X = borderRect.Right;
            var resultRect = new Rectangle(leftDownCorner, rectangleSize);
            return resultRect;
        }

        private Rectangle PutRectangleToTheRight(Size rectangleSize, Rectangle lastRect)
        {
            var rightDownCorner = new Point(lastRect.Right, lastRect.Bottom);
            var newRectLocation = new Point(rightDownCorner.X,
                rightDownCorner.Y - rectangleSize.Height);
            var borderRect = Rectangles.Where(rect =>
                    rect.Left < newRectLocation.X + rectangleSize.Width && newRectLocation.X < rect.Right)
                .OrderBy(rect => rect.Top)
                .FirstOrDefault();
            if (!borderRect.IsEmpty)
                newRectLocation.Y = borderRect.Top - rectangleSize.Height;
            var resultRect = new Rectangle(newRectLocation, rectangleSize);
            return resultRect;
        }

        private Rectangle PutRectangleAbove(Size rectangleSize, Rectangle lastRect)
        {
            var rightUpperCorner = new Point(lastRect.Right, lastRect.Top);
            var newRectLocation = new Point(rightUpperCorner.X - rectangleSize.Width,
                rightUpperCorner.Y - rectangleSize.Height);
            var borderRect = Rectangles.Where(rect =>
                    rect.Y < newRectLocation.Y + rectangleSize.Height && newRectLocation.Y < rect.Bottom)
                .OrderBy(rect => rect.Left)
                .FirstOrDefault();
            if (!borderRect.IsEmpty)
                newRectLocation.X = borderRect.Left - rectangleSize.Width;
            var resultRect = new Rectangle(newRectLocation, rectangleSize);
            return resultRect;
        }
    }
}