using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public List<Rectangle> Rectangles { get; set; }
        private readonly Point center;
        private readonly double offsetPoint;
        private readonly double spiralStep;
        private int lastNumberPoint;
        private readonly bool isOffsetToCenter;

        public CircularCloudLayouter(Point center, bool isOffsetToCenter, double offsetPoint, double spiralStep)
        {
            if (center.X < 0 || center.Y < 0) throw new ArgumentException();
            this.spiralStep = spiralStep;
            this.offsetPoint = offsetPoint;
            this.center = center;
            this.isOffsetToCenter = isOffsetToCenter;
            Rectangles = new List<Rectangle>();
            lastNumberPoint = 0;
        }

        public CircularCloudLayouter(Point center) : this(center, false, 0.01, -0.3)
        {
        }

        public CircularCloudLayouter(Point center, bool isOffsetToCenter) : this(center, isOffsetToCenter, 0.01, -0.3)
        {
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle rect;
            for (; ; lastNumberPoint++)
            {
                var phi = lastNumberPoint * spiralStep;
                var r = offsetPoint * lastNumberPoint;
                var x = (int)(r * Math.Cos(phi)) + center.X;
                var y = (int)(r * Math.Sin(phi)) + center.Y;
                var point = new Point(x - rectangleSize.Width / 2, y - rectangleSize.Height / 2);
                rect = new Rectangle(point, rectangleSize);
                if (!rect.AreIntersected(Rectangles))
                {
                    if (isOffsetToCenter) rect = OffsetToCenter(rect);
                    break;
                }
            }
            Rectangles.Add(rect);
            return rect;
        }

        private Rectangle OffsetToCenter(Rectangle rect)
        {
            var point = rect.Location;
            while (center.X - 1 > rect.Center().X || center.X + 1 < rect.Center().X)
            {
                var newX = ((rect.Center().X < center.X) ? 1 : -1) + point.X;
                var pointNew = new Point(newX, point.Y);
                var rectNew = new Rectangle(pointNew, rect.Size);
                if (rectNew.AreIntersected(Rectangles)) break;
                point = pointNew;
                rect = rectNew;
            }
            while (center.Y - 1 > rect.Center().Y || center.Y + 1 < rect.Center().Y)
            {
                var newY = ((rect.Center().Y < center.Y) ? 1 : -1) + point.Y;
                var pointNew = new Point(point.X, newY);
                var rectNew = new Rectangle(pointNew, rect.Size);
                if (rectNew.AreIntersected(Rectangles)) break;
                point = pointNew;
                rect = rectNew;
            }
            return rect;
        }
    }
}
