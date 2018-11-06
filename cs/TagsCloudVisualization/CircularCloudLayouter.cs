using System;
using System.Collections.Generic;
using System.Drawing;

namespace TagsCloudVisualization
{
    public class CircularCloudLayouter
    {
        private readonly List<Rectangle> placedRectangles;
        private readonly ArchimedeanSpiralGenerator spiralGenerator;

        public CircularCloudLayouter(Point center)
        {
            placedRectangles = new List<Rectangle>();
            spiralGenerator = new ArchimedeanSpiralGenerator(center, 1, (float)(1 / (2 * Math.PI)));
        }

        public Rectangle PutNextRectangle(Size rectangleSize)
        {
            Rectangle nextRectangle;
            do
            {
                var nextPoint = spiralGenerator.GetNextPoint();
                nextRectangle = GetRectangleWithCenterIn(
                    new Point((int)nextPoint.X, (int)nextPoint.Y), rectangleSize);
            } while (nextRectangle.IntersectsWithAny(placedRectangles));

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
