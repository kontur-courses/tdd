using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;
        public List<Rectangle> Rectangles { get; private set; }
        private double _dr = 0.01; // delta radius in SpiralFunction
        private double _fi = 0.0368; // angle in SpiralFunction

        public CircularCloudLayouter(Point center)
        {
            Center = center;
            Rectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
                throw new ArgumentException("only positive size");
            var rect = FindFreePlaceForNewRectangle(rectangleSize);
            TryMoveRectangleCloserToCenter(ref rect);
            Rectangles.Add(rect);
            return rect;
        }

        private void TryMoveRectangleCloserToCenter(ref Rectangle rect)
        {
            var rectCenter = rect.GetCenter();
            if (rectCenter.X != Center.X)
            {
                var dx = (-1) * Math.Abs(rectCenter.X - Center.X) / (rectCenter.X - Center.X);
                while (NotIntersectOthers(rect) && rect.GetCenter().X != Center.X)
                {
                    rect.X += dx;
                }
                if (!NotIntersectOthers(rect))
                    rect.X -= dx;
            }
            if (rectCenter.Y != Center.Y)
            {
                var dy = (-1) * Math.Abs(rectCenter.Y - Center.Y) / (rectCenter.Y - Center.Y);
                while (NotIntersectOthers(rect) && rect.GetCenter().Y != Center.Y)
                {
                    rect.Y += dy;
                }
                if (!NotIntersectOthers(rect))
                    rect.Y -= dy;
            }
        }

        private Rectangle FindFreePlaceForNewRectangle(Size rectangleSize)
        {
            var arg = 0;
            while (true)
            {
                var rectCenter = SpiralFunction(arg);
                var rect = new Rectangle()
                {
                    Size = rectangleSize,
                    X = rectCenter.X - rectangleSize.Width / 2,
                    Y = rectCenter.Y - rectangleSize.Height / 2,
                };
                if (NotIntersectOthers(rect))
                    return rect;
                arg++;
            }
        }

        private bool NotIntersectOthers(Rectangle rect)
        {
            foreach (var r in Rectangles)
                if (rect.IntersectsWith(r))
                    return false;
            return true;
        }

        private Point SpiralFunction(int arg)
        {
            return new Point(
                (int)(Center.X + _dr * arg * Math.Cos(_fi * arg)),
                (int)(Center.Y + _dr * arg * Math.Sin(_fi * arg)));
        }
    }
}
