using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        public readonly Point Center;

        public HashSet<Rectangle> Rectangles { get; private set; } = new HashSet<Rectangle>();

        public CircularCloudLayouter(Point center)
        {
            Center = center;
        }

        public Rectangle PutNextRectangle(Size rectSize)
        {
            ThrowExceptionOnWrongSize(rectSize);

            var result = new Rectangle(ChooseRectangleLocation(rectSize), rectSize);
            Rectangles.Add(result);
            return result;
        }

        private Point ChooseRectangleLocation(Size rectSize)
        {
            var resultCenter = new Point();
            foreach (var point in GenerateSpiralPoints())
                if (!DefineRectangle(point, rectSize).IntersectsWithAny(Rectangles))
                {
                    resultCenter = point;
                    break;
                }

            return CountLocation(resultCenter, rectSize);
        }

        private IEnumerable<Point> GenerateSpiralPoints()
        {
            var angle = 0.0;
            var curPoint = Center;
            while (true)
            {
                yield return curPoint;
                angle += Math.PI / 16;
                var x = (int)(angle * Math.Cos(angle) / (2 * Math.PI)) + Center.X;
                var y = (int)(angle * Math.Sin(angle) / (2 * Math.PI)) + Center.Y;
                curPoint = new Point(x, y);
            }
        }

        private Rectangle DefineRectangle(Point rectCenter, Size rectSize)
        {
            return new Rectangle(CountLocation(rectCenter, rectSize), rectSize);
        }

        private Point CountLocation(Point rectCenter, Size rectSize)
        {
            var resultX = rectCenter.X - rectSize.Width / 2;
            var resultY = rectCenter.Y - rectSize.Height / 2;
            return new Point(resultX, resultY);
        }

        private void ThrowExceptionOnWrongSize(Size rectSize)
        {
            if (rectSize.Height <= 0 || rectSize.Width <= 0)
                throw new ArgumentException(rectSize.ToString());
        }
    }
}
