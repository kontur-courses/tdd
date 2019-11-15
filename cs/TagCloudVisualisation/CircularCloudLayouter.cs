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

                var minx = Rectangles.Select(r => r.X).Min();
                var miny = Rectangles.Select(r => r.Y).Min();
                return new Point(minx, miny);
            }
        }
        public Point RightUpperPointOfCloud
        {
            get
            {
                if (Rectangles.Count == 0)
                    return new Point(0, 0);

                var maxx = Rectangles.Select(r => r.X + r.Width).Max();
                var maxy = Rectangles.Select(r => r.Y + r.Height).Max();
                return new Point(maxx, maxy);
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

            var rect = new Rectangle(spiralPointer.GetSpiralNext(), rectangleSize);
            while (Rectangles.Any(currentR => currentR.IntersectsWith(rect)))
            {
                var currentPoint = spiralPointer.GetSpiralNext();
                rect.X = currentPoint.X;
                rect.Y = currentPoint.Y;
            }
            //метод двигает точку если возможно
            while (TryMoveToCenter(out rect, rect)) ;
            if (Math.Abs(Math.Log2(Rectangles.Count) % 3) < 0.005f)
                spiralPointer.Reset();
            Rectangles.Add(rect);
            return rect;
        }

        private bool TryMoveToCenter(out Rectangle rectOut, Rectangle rect)
        {
            if (rect.X == center.X && rect.Y == center.Y)
            {
                rectOut = rect;
                return false;
            }

            var dx = center.X - rect.X;
            var dy = center.Y - rect.Y;

            var canMoveX = dx == 0 ?
                            false :
                            dx > 0 ?
                            CanMove(out rect, rect, 1, 0) :
                            CanMove(out rect, rect, -1, 0);
            var canMoveY = dy == 0 ?
                            false :
                            dy > 0 ?
                            CanMove(out rect, rect, 0, 1) :
                            CanMove(out rect, rect, 0, -1);
            rectOut = rect;
            return canMoveX || canMoveY;
        }
        private bool CanMove(out Rectangle rectOut, Rectangle rect, int dx, int dy)
        {
            rect.X += dx;
            rect.Y += dy;
            rectOut = rect;

            if (!Rectangles.Any(r => r.IntersectsWith(rect)))
                return true;

            rectOut.X -= dx;
            rectOut.Y -= dy;

            return false;
        }
    }
}
