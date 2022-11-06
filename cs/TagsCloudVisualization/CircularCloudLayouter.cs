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

        public CircularCloudLayouter(Point center, double offsetPoint, double spiralStep)
        {
            if (center.X < 0 || center.Y < 0) throw new ArgumentException();
            this.spiralStep = spiralStep;
            this.offsetPoint = offsetPoint;
            this.center = center;
            Rectangles = new List<Rectangle>();
            lastNumberPoint = 0;
        }

        public CircularCloudLayouter(Point center) : this(center, 0.01, -0.3)
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
                if (!rect.AreIntersected(Rectangles)) break;
            }
            Rectangles.Add(rect);
            return rect;
        }
    }
}
