using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace TagCloud
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        private readonly FermaSpiral spiralPointer;
        public readonly List<Rectangle> Rectangles;
        public Size SizeOfCloud
        {
            get
            {
                if (Rectangles.Count == 0) return new Size(0, 0);
                return new Size(RightUpperPointOfCloud.X - LeftDownPointOfCloud.X,
                    RightUpperPointOfCloud.Y - LeftDownPointOfCloud.Y);
            }
        }
        public Point LeftDownPointOfCloud
        {
            get
            {
                if (Rectangles.Count == 0) return new Point(0, 0);
                var minx = Rectangles.Select(r => r.X).Min();
                var miny = Rectangles.Select(r => r.Y).Min();
                return new Point(minx, miny);
            }
        }
        public Point RightUpperPointOfCloud
        {
            get
            {
                if (Rectangles.Count == 0) return new Point(0, 0);
                var maxx = Rectangles.Select(r => r.X+r.Width).Max();
                var maxy = Rectangles.Select(r => r.Y+r.Height).Max();
                return new Point(maxx, maxy);
            }
        }
        public CircularCloudLayouter(Point center)
        {
            Rectangles = new List<Rectangle>();
            Center = center;
            spiralPointer = new FermaSpiral(1, center);

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
            if((int)Math.Log(Rectangles.Count)%4==0)
                spiralPointer.Reset();
            Rectangles.Add(rect);
            return rect;
        }
    }
}