using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace TagsCloudVisualization
{
    class CircularCloudLayouter
    {
        private readonly Point center;
        private List<Rectangle> placedRectangles;
        private IEnumerator<PointF> pointsEnumerator;

        public CircularCloudLayouter(Point center)
        {
            this.center = center;
            placedRectangles = new List<Rectangle>();
            pointsEnumerator = ArchimedeanSpiralGenerator.
                GetArchimedeanSpiralGenerator(center, 1, (float)(1 / (2 * Math.PI))).GetEnumerator();
            pointsEnumerator.MoveNext();
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            var nextPoint = pointsEnumerator.Current;
            var nextRectangle =
                GetRectangleWithCenterIn(new Point((int) nextPoint.X, (int) nextPoint.Y), rectangleSize);
            while (nextRectangle.IntersectsWithAny(placedRectangles))
            {
                pointsEnumerator.MoveNext();
                nextPoint = pointsEnumerator.Current;
                nextRectangle =
                    GetRectangleWithCenterIn(new Point((int) nextPoint.X, (int) nextPoint.Y), rectangleSize);
            }
            placedRectangles.Add(nextRectangle);

            return nextRectangle;
        }

        private Rectangle GetRectangleWithCenterIn(Point rectangleCenter, Size rectangleSize)
        {
            return new Rectangle(
                new Point(rectangleCenter.X - rectangleSize.Width / 2,
                          rectangleCenter.Y - rectangleSize.Height / 2), rectangleSize);
        }
    }
}
