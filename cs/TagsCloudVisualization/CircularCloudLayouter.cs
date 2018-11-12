using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private Point cloudCenter;
        private List<Rectangle> cloudRectangles;

        public CircularCloudLayouter(Point center)
        {
            cloudCenter = center;
            cloudRectangles = new List<Rectangle>();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var rectangle = new Rectangle();

            rectangle.Size = rectangleSize;
            rectangle.Location = FindEmptyLocation(rectangleSize);

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

                if (cloudRectangles.TrueForAll(rect => !rect.IntersectsWith(rectangle)))
                    return rectangle.Location;
            }
            throw new Exception("Didn't find position for rectangle");
        }

        private static IEnumerable<Point> SpiralPositions(Point center)
        {
            double spiralStepCoiff = 1.5;

            for (double fi = 0; ; fi += Math.PI / 180)
            {
                var r = spiralStepCoiff * fi;
                var x = r * Math.Cos(fi) + center.X;
                var y = r * Math.Sin(fi) + center.Y;

                yield return new Point((int) x, (int) y);
            }
        }

        private void shiftX(ref Rectangle rect, int dx)
        {
            bool canMove = true;
            while (canMove)
            {
                rect.X += dx;
                foreach (var tag in cloudRectangles)
                    if (tag.IntersectsWith(rect) || (rect.X + rect.Width / 2) == cloudCenter.X)
                    {
                        canMove = false;
                        rect.X -= dx;
                        break;
                    }
            }
        }

        private void shiftY(ref Rectangle rect, int dy)
        {
            bool canMove = true;
            while (canMove)
            {
                rect.Y += dy;
                foreach (var tag in cloudRectangles)
                    if (tag.IntersectsWith(rect) || (rect.Y+ rect.Height / 2) == cloudCenter.Y)
                    {
                        canMove = false;
                        rect.Y -= dy;
                        break;
                    }
            }
        }

        private void Compact(ref Rectangle rect)
        {
            if (cloudRectangles.Count == 0)
                return;

            var rectangleCenter = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            var dx = Math.Sign(cloudCenter.X - rectangleCenter.X);
            var dy = Math.Sign(cloudCenter.Y - rectangleCenter.Y);

            shiftX(ref rect, dx);
            shiftY(ref rect, dy);
        }
    }
}