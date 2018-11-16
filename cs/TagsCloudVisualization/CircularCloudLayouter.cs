using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private const double spiralStepCoiff = 1.5;
        private Point cloudCenter;
        private List<Rectangle> cloudRectangles;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            cloudRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle
            {
                Size = rectangleSize,
                Location = FindEmptyLocation(rectangleSize)
            };

            Compact(ref rectangle);

            cloudRectangles.Add(rectangle);
            return rectangle;
        }

        private Point FindEmptyLocation(Size size)
        {
            var rectangle = new Rectangle(Point.Empty, size);

            foreach (Point position in SpiralPositions(cloudCenter))
            {
                rectangle.Location = new Point(position.X - rectangle.Width / 2, position.Y - rectangle.Height / 2);

                if (!rectangle.IntersectsWithAny(cloudRectangles))
                    return rectangle.Location;
            }
            throw new Exception("Didn't find position for rectangle");
        }

        private static IEnumerable<Point> SpiralPositions(Point center)
        {
            for (double fi = 0; ; fi += Math.PI / 180)
            {
                var r = spiralStepCoiff * fi;
                var x = r * Math.Cos(fi) + center.X;
                var y = r * Math.Sin(fi) + center.Y;

                yield return new Point((int) x, (int) y);
            }
        }

        private bool Shift(ref Rectangle rect, int shiftX, int shiftY)
        {
            if (shiftX == 0 && shiftY == 0)
                return false;

            rect.X += shiftX;
            rect.Y += shiftY;

            if (rect.IntersectsWithAny(cloudRectangles))
            {
                rect.X -= shiftX;
                rect.Y -= shiftY;
                return false;
            }
            return true;
        }

        private void Compact(ref Rectangle rect)
        {
            if (cloudRectangles.Count == 0)
                return;

            while (Shift(ref rect, Math.Sign(cloudCenter.X - rect.X), 0) ||
                   Shift(ref rect, 0, Math.Sign(cloudCenter.Y - rect.Y))) ;
        }
    }
}