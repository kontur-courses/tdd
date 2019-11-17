using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly FermaSpiral spiralPointer;
        public readonly List<Rectangle> Rectangles;
        private readonly Point center;

        public Size SizeOfCloud =>
            new Size(RightUpperPointOfCloud.X - LeftDownPointOfCloud.X,
                RightUpperPointOfCloud.Y - LeftDownPointOfCloud.Y);

        public Point LeftDownPointOfCloud
        {
            get
            {
                if (Rectangles.Count == 0)
                    return new Point(0, 0);

                var minX = Rectangles.Select(r => r.X).Min();
                var minY = Rectangles.Select(r => r.Y).Min();
                return new Point(minX, minY);
            }
        }

        public Point RightUpperPointOfCloud
        {
            get
            {
                if (Rectangles.Count == 0)
                    return new Point(0, 0);

                var maxX = Rectangles.Select(r => r.X + r.Width).Max();
                var maxY = Rectangles.Select(r => r.Y + r.Height).Max();
                return new Point(maxX, maxY);
            }
        }

        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            spiralPointer = new FermaSpiral(1, center);
            this.center = center;
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.IsEmpty || rectangleSize.Height <= 0 || rectangleSize.Width <= 0)
                throw new ArgumentException("Rectangle does not exist");

            var rect = new Rectangle(spiralPointer.GetSpiralCurrent(), rectangleSize);
            while (Rectangles.Any(currentR => currentR.IntersectsWith(rect)))
            {
                var currentPoint = spiralPointer.GetSpiralNext();
                rect.X = currentPoint.X;
                rect.Y = currentPoint.Y;
            }

            var canMoveToCenter = true;
            while (canMoveToCenter)
            {
                rect = TryMoveToCenter(out canMoveToCenter, rect);
            }

            Rectangles.Add(rect);
            return rect;
        }

        private Rectangle TryMoveToCenter(out bool canMove, Rectangle rect)
        {
            if (rect.X == center.X && rect.Y == center.Y)
            {
                canMove = false;
                return rect;
            }

            var dx = center.X - rect.X;
            var dy = center.Y - rect.Y;
            var canMoveX = false;
            var canMoveY = false;
            rect = dx == 0 ? rect :
                dx > 0 ? TryToMove(out canMoveX, rect, 1, 0) :
                TryToMove(out canMoveX, rect, -1, 0);
            rect = dy == 0 ? rect :
                dy > 0 ? TryToMove(out canMoveY, rect, 0, 1) :
                TryToMove(out canMoveY, rect, 0, -1);
            canMove = canMoveX || canMoveY;
            return rect;
        }

        private Rectangle TryToMove(out bool canMove, Rectangle rect, int dx, int dy)
        {
            rect.X += dx;
            rect.Y += dy;
            canMove = !Rectangles.Any(r => r.IntersectsWith(rect));
            if (!canMove)
            {
                rect.X -= dx;
                rect.Y -= dy;
            }

            return rect;
        }
    }
}